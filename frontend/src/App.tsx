import { Navigate, Route, Routes } from "react-router-dom";
import Products from "./pages/Products";
import Lists from "./pages/Lists";
import ListDetail from "./pages/ListDetail";
import {
  AdminProtectedRoute,
  ProtectedRoute,
} from "./components/ProtectedRoute";
import Layout from "./components/Layout";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import { ToastContainer } from "react-toastify";
import AnonymousRoute from "./components/AnonymousRoute";
import AdminProducts from "./pages/AdminProducts";
import AdminCategories from "./pages/AdminCategories";

function App() {
  return (
    <>
      <ToastContainer
        position="top-center"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnHover
        theme="light"
      />
      <Routes>
        <Route path="/" element={<Navigate to="/login" />} />
        <Route element={<ProtectedRoute />}>
          <Route path="/app" element={<Layout />}>
            <Route path="admin" element={<AdminProtectedRoute />}>
              <Route path="products" element={<AdminProducts />} />
              <Route path="categories" element={<AdminCategories />} />
            </Route>

            <Route path="products" element={<Products />} />
            <Route path="lists" element={<Lists />} />
            <Route path="lists/:id" element={<ListDetail />} />
          </Route>
        </Route>
        <Route element={<AnonymousRoute />}>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
        </Route>
      </Routes>
    </>
  );
}

export default App;
