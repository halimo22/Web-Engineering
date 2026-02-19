from typing import Dict, List, Optional
from fastapi import FastAPI, HTTPException, Query, Path
from pydantic import BaseModel, Field, EmailStr

app = FastAPI(title="School Program API (HTTP Methods Demo)")

# ----------------------------
# Models
# ----------------------------
class StudentCreate(BaseModel):
    name: str = Field(..., min_length=2)
    email: EmailStr
    grade_level: int = Field(..., ge=1, le=12)

class StudentUpdate(BaseModel):  # for PUT (full replacement)
    name: str = Field(..., min_length=2)
    email: EmailStr
    grade_level: int = Field(..., ge=1, le=12)

class StudentPatch(BaseModel):  # for PATCH (partial update)
    name: Optional[str] = Field(None, min_length=2)
    email: Optional[EmailStr] = None
    grade_level: Optional[int] = Field(None, ge=1, le=12)

class CourseCreate(BaseModel):
    title: str = Field(..., min_length=2)
    teacher: str = Field(..., min_length=2)
    capacity: int = Field(..., ge=1, le=500)

class EnrollmentCreate(BaseModel):
    student_id: int = Field(..., ge=1)
    course_id: int = Field(..., ge=1)

# ----------------------------
# In-memory "database"
# ----------------------------
students: Dict[int, dict] = {}
courses: Dict[int, dict] = {}
enrollments: Dict[int, dict] = {}

student_seq = 1
course_seq = 1
enrollment_seq = 1


def next_id(seq_name: str) -> int:
    global student_seq, course_seq, enrollment_seq
    if seq_name == "student":
        student_seq += 1
        return student_seq - 1
    if seq_name == "course":
        course_seq += 1
        return course_seq - 1
    enrollment_seq += 1
    return enrollment_seq - 1


# =========================================================
# STUDENTS: demonstrate GET / POST / PUT / PATCH / DELETE
# =========================================================

# GET (Read collection) + query params (filter/pagination demo)
@app.get("/students")
def list_students(
    q: Optional[str] = Query(None, description="Search by name (contains)"),
    grade_level: Optional[int] = Query(None, ge=1, le=12),
    limit: int = Query(50, ge=1, le=200),
    offset: int = Query(0, ge=0),
):
    data = list(students.values())

    if q:
        q_lower = q.lower()
        data = [s for s in data if q_lower in s["name"].lower()]

    if grade_level is not None:
        data = [s for s in data if s["grade_level"] == grade_level]

    return {
        "method": "GET",
        "resource": "students",
        "count": len(data),
        "items": data[offset : offset + limit],
    }


# GET (Read single)
@app.get("/students/{student_id}")
def get_student(student_id: int = Path(..., ge=1)):
    student = students.get(student_id)
    if not student:
        raise HTTPException(404, detail="Student not found")
    return {"method": "GET", "resource": "students", "item": student}


# POST (Create)
@app.post("/students", status_code=201)
def create_student(payload: StudentCreate):
    sid = next_id("student")
    student = {"id": sid, **payload.model_dump()}
    students[sid] = student
    return {"method": "POST", "resource": "students", "created": student}


# PUT (Replace entire student)
@app.put("/students/{student_id}")
def replace_student(student_id: int = Path(..., ge=1), payload: StudentUpdate = None):
    if student_id not in students:
        raise HTTPException(404, detail="Student not found")
    updated = {"id": student_id, **payload.model_dump()}
    students[student_id] = updated
    return {"method": "PUT", "resource": "students", "replaced": updated}


# PATCH (Partial update)
@app.patch("/students/{student_id}")
def patch_student(student_id: int = Path(..., ge=1), payload: StudentPatch = None):
    student = students.get(student_id)
    if not student:
        raise HTTPException(404, detail="Student not found")

    patch_data = payload.model_dump(exclude_unset=True)
    student.update(patch_data)
    students[student_id] = student
    return {"method": "PATCH", "resource": "students", "patched": student}


# DELETE (Remove)
@app.delete("/students/{student_id}", status_code=204)
def delete_student(student_id: int = Path(..., ge=1)):
    if student_id not in students:
        raise HTTPException(404, detail="Student not found")

    # Remove related enrollments too (simple cascading delete)
    to_delete = [eid for eid, e in enrollments.items() if e["student_id"] == student_id]
    for eid in to_delete:
        enrollments.pop(eid, None)

    students.pop(student_id)
    return


# =========================================================
# COURSES: GET / POST / DELETE (simple demo)
# =========================================================
@app.get("/courses")
def list_courses():
    return {"method": "GET", "resource": "courses", "items": list(courses.values())}

@app.post("/courses", status_code=201)
def create_course(payload: CourseCreate):
    cid = next_id("course")
    course = {"id": cid, **payload.model_dump()}
    courses[cid] = course
    return {"method": "POST", "resource": "courses", "created": course}

@app.delete("/courses/{course_id}", status_code=204)
def delete_course(course_id: int = Path(..., ge=1)):
    if course_id not in courses:
        raise HTTPException(404, detail="Course not found")

    # Remove related enrollments
    to_delete = [eid for eid, e in enrollments.items() if e["course_id"] == course_id]
    for eid in to_delete:
        enrollments.pop(eid, None)

    courses.pop(course_id)
    return


# =========================================================
# ENROLLMENTS: POST creates relationship, GET reads
# =========================================================
@app.get("/enrollments")
def list_enrollments():
    return {"method": "GET", "resource": "enrollments", "items": list(enrollments.values())}

@app.post("/enrollments", status_code=201)
def enroll_student(payload: EnrollmentCreate):
    if payload.student_id not in students:
        raise HTTPException(400, detail="student_id does not exist")
    if payload.course_id not in courses:
        raise HTTPException(400, detail="course_id does not exist")

    # prevent duplicates
    for e in enrollments.values():
        if e["student_id"] == payload.student_id and e["course_id"] == payload.course_id:
            raise HTTPException(409, detail="Student already enrolled in this course")

    eid = next_id("enrollment")
    enrollment = {"id": eid, **payload.model_dump()}
    enrollments[eid] = enrollment
    return {"method": "POST", "resource": "enrollments", "created": enrollment}

@app.delete("/enrollments/{enrollment_id}", status_code=204)
def drop_enrollment(enrollment_id: int = Path(..., ge=1)):
    if enrollment_id not in enrollments:
        raise HTTPException(404, detail="Enrollment not found")
    enrollments.pop(enrollment_id)
    return


# =========================================================
# "ACTION" endpoints (non-CRUD) — still REST-friendly
# =========================================================
@app.post("/students/{student_id}/promote")
def promote_student(student_id: int = Path(..., ge=1)):
    student = students.get(student_id)
    if not student:
        raise HTTPException(404, detail="Student not found")
    if student["grade_level"] >= 12:
        raise HTTPException(400, detail="Student already at max grade level")

    student["grade_level"] += 1
    return {"method": "POST", "action": "promote", "result": student}