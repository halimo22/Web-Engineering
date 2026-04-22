import { useState } from "react";
import "./App.css";
function App() {

  const [task,setTask] = useState("");
  const [tasks,setTasks] = useState([]);
  const handleAddTask = () => {
    setTasks([...tasks,task]);
    setTask("");
  }
  const handleDeleteTask = (deleteIndex) => {
  const updatedTasks = tasks.filter((item, index) => index !== deleteIndex);
  setTasks(updatedTasks);
};

  return (
    <div>
      <h1>Todo App</h1>
      <input type="text" placeholder="Enter Task" value={task} onChange={(e) => setTask(e.target.value)} />
      <button onClick={handleAddTask}>Add Task</button>
      <ul>
        {tasks.map((item, index) => (
          <li style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }} key={index}>
            {item}
            <button style={{ marginLeft: "10px" }} onClick={() => handleDeleteTask(index)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;