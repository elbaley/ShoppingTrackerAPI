import { useAuth } from "@/lib/useAuth";
import { Navigate, Outlet } from "react-router-dom";

export const ProtectedRoute = () => {
  const { user } = useAuth();
  if (!user) {
    console.log("there is no user!!");
    // user is not authenticated
    return <Navigate to="/login" />;
  }
  return <Outlet />;
};

export const AdminProtectedRoute = () => {
  const { user } = useAuth();
  if (!user || user.role !== "Admin") {
    console.log("User is not admin!");
    // user is not admin
    return <Navigate to="/app/products" />;
  }
  return <Outlet />;
};
