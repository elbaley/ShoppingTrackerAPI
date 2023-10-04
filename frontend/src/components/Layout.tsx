import { Fragment, PropsWithChildren } from "react";
import { IoPerson } from "react-icons/io5";
import Sidebar from "./Sidebar";
import { BiLogOut } from "react-icons/bi";
import { Link, Outlet } from "react-router-dom";
import { useAuth } from "@/lib/useAuth";
import { Menu, Transition } from "@headlessui/react";
import { adminSidebarLinks, sidebarLinks } from "@/lib/sidebarLinks";

interface LayoutProps {}

const Layout = ({}: PropsWithChildren<LayoutProps>) => {
  return (
    <div id="dashboardLayout" className="">
      <Navbar />
      <ProfileMenu />
      <Sidebar />
      <div id="dashboardContentSafe" className="sm:ml-24 pt-16 sm:pt-12">
        <Outlet />
      </div>
    </div>
  );
};

const Navbar = () => {
  return (
    <header className="shadow-sm shadow-zinc-300 sm:hidden pl-2 h-14 fixed w-full flex justify-between items-center z-20 bg-white dark:bg-black dark:bg-opacity-10 bg-opacity-10 backdrop-blur-md">
      <h3 className="text-black font-extrabold">Shopping Tracker</h3>
    </header>
  );
};

function ProfileMenu() {
  const { user, logout } = useAuth();

  return (
    <div className="fixed sm:absolute right-0 mr-4 z-50 mt-1 ">
      <Menu as="div" className="relative inline-block text-left">
        <div>
          <Menu.Button>
            <img
              className="h-12 rounded-full"
              src={`https://placehold.co/400x400?font=roboto&text=${user.name[0]}${user.name[1]}`}
            />
          </Menu.Button>
        </div>
        <Transition
          as={Fragment}
          enter="transition ease-out duration-100"
          enterFrom="transform opacity-0 scale-95"
          enterTo="transform opacity-100 scale-100"
          leave="transition ease-in duration-75"
          leaveFrom="transform opacity-100 scale-100"
          leaveTo="transform opacity-0 scale-95"
        >
          <Menu.Items className="absolute right-0  w-56 origin-top-right divide-y divide-gray-100 rounded-md bg-white dark:bg-darkSecondary shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
            <div className="px-1 py-1">
              {user.role === "User"
                ? sidebarLinks.map((link) => {
                    return (
                      <Menu.Item key={`${link.label}--link`}>
                        <Link className="sm:hidden" to={link.href}>
                          <button className="hover:bg-gray-400 hover:text-white group flex w-full items-center rounded-md px-2 py-2 text-sm">
                            {link.label}
                          </button>
                        </Link>
                      </Menu.Item>
                    );
                  })
                : adminSidebarLinks.map((link) => {
                    return (
                      <Menu.Item key={`${link.label}--link`}>
                        <Link className="sm:hidden" to={link.href}>
                          <button className="hover:bg-gray-400 hover:text-white group flex w-full items-center rounded-md px-2 py-2 text-sm">
                            {link.label}
                          </button>
                        </Link>
                      </Menu.Item>
                    );
                  })}
              <Menu.Item>
                <button className="hover:bg-gray-400 hover:text-white group flex w-full items-center rounded-md px-2 py-2 text-sm">
                  <IoPerson className="mr-2 h-5 w-5 text-black dark:text-white" />
                  {user.name}
                </button>
              </Menu.Item>
            </div>
            <div className="px-1 py-1">
              <Menu.Item>
                <button
                  onClick={logout}
                  className="hover:bg-gray-400 hover:text-white group flex w-full items-center rounded-md px-2 py-2 text-sm"
                >
                  <BiLogOut className="mr-2 h-5 w-5 text-black dark:text-white" />
                  Logout
                </button>
              </Menu.Item>
            </div>
          </Menu.Items>
        </Transition>
      </Menu>
    </div>
  );
}

export default Layout;
