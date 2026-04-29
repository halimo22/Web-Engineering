import { useEffect, useState } from 'react'
import axios from 'axios'
import { Link, Navigate, Route, Routes, useParams } from 'react-router-dom'
import './App.css'

const api = axios.create({
  baseURL: 'https://jsonplaceholder.typicode.com',
})

function UsersPage() {
  const [users, setUsers] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const response = await api.get('/users')
        setUsers(response.data)
      } catch {
        setError('Failed to load users. Please try again.')
      } finally {
        setLoading(false)
      }
    }

    fetchUsers()
  }, [])

  return (
    <section>
      <h2>All Users</h2>
      <p className="hint">GET /users</p>

      {loading && <p>Loading users...</p>}
      {error && <p className="error">{error}</p>}

      {!loading && !error && (
        <ul className="user-list">
          {users.map((user) => (
            <li key={user.id}>
              <Link to={`/users/${user.id}`}>{user.name}</Link>
              <span>{user.email}</span>
            </li>
          ))}
        </ul>
      )}
    </section>
  )
}

function UserDetailsPage() {
  const { id } = useParams()
  const [user, setUser] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await api.get(`/users/${id}`)
        setUser(response.data)
      } catch {
        setError('Failed to load user details. Please try again.')
      } finally {
        setLoading(false)
      }
    }

    fetchUser()
  }, [id])

  return (
    <section>
      <h2>User Details</h2>
      <p className="hint">GET /users/{id}</p>

      {loading && <p>Loading user details...</p>}
      {error && <p className="error">{error}</p>}

      {!loading && !error && user && (
        <article className="user-card">
          <h3>{user.name}</h3>
          <p>
            <strong>Username:</strong> {user.username}
          </p>
          <p>
            <strong>Email:</strong> {user.email}
          </p>
          <p>
            <strong>Phone:</strong> {user.phone}
          </p>
          <p>
            <strong>Website:</strong> {user.website}
          </p>
        </article>
      )}
    </section>
  )
}

function App() {
  return (
    <main className="app">
      <header>
        <h1>Axios + React Router Demo</h1>
        <p>Simple example for fetching data and navigating between routes.</p>
      </header>

      <nav className="nav-links">
        <Link to="/users">Users List</Link>
        <Link to="/users/1">User #1</Link>
      </nav>

      <Routes>
        <Route path="/" element={<Navigate to="/users" replace />} />
        <Route path="/users" element={<UsersPage />} />
        <Route path="/users/:id" element={<UserDetailsPage />} />
      </Routes>
    </main>
  )
}

export default App