import { useAuth } from "@/lib/useAuth";
import { Outlet, Navigate } from "react-router-dom";
interface AnonymousRouteProps {}

const AnonymousRoute = ({}: AnonymousRouteProps) => {
  const { user } = useAuth();
  return user ? <Navigate to="/products" replace /> : <Outlet />;
};

export default AnonymousRoute;
