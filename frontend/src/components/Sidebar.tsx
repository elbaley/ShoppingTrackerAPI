interface SidebarProps {}
import {
  adminSidebarLinks,
  sidebarIcons,
  sidebarLinks,
} from "@/lib/sidebarLinks";
import { useAuth } from "@/lib/useAuth";
import clsx from "clsx";
import { IconType } from "react-icons";
import { Link } from "react-router-dom";

const Sidebar = ({}: SidebarProps) => {
  const { user } = useAuth();
  return (
    <aside className="sm:visible invisible flex flex-col h-screen w-24 fixed bg-white dark:bg-darkPrimary rounded-tr-[1.25rem] rounded-br-[1.25rem] pt-10 mr-24 dark:border-r dark:border-y dark:border-black overflow-scroll">
      <span className="text-black text-center font-extrabold text-lg">ST</span>
      <hr className="w-[70px] mx-auto border-stone-300 dark:border-zinc-900 mt-7" />
      <div
        id="sidebar__container"
        className="flex-1 flex flex-col gap-9 items-center mt-8 "
      >
        {user.role === "User"
          ? sidebarLinks.map((link) => {
              const Icon: IconType = sidebarIcons[link.icon];
              const isActive = true;
              const isSettings = link.label === "Settings";
              return (
                <Link
                  key={link.href}
                  to={link.href}
                  className={`cursor-pointer ${isSettings && "mt-auto mb-8 "}`}
                >
                  <Icon
                    size={28}
                    className={clsx(
                      isActive
                        ? "text-sidebarBtn-active dark:text-dark-sidebarBtn-active"
                        : "text-sidebarBtn-inactive dark:text-dark-sidebarBtn-inactive",
                    )}
                  />
                </Link>
              );
            })
          : adminSidebarLinks.map((link) => {
              const Icon: IconType = sidebarIcons[link.icon];
              const isActive = true;
              const isSettings = link.label === "Settings";
              return (
                <Link
                  key={link.href}
                  to={link.href}
                  className={`cursor-pointer ${isSettings && "mt-auto mb-8 "}`}
                >
                  <Icon
                    size={28}
                    className={clsx(
                      isActive
                        ? "text-sidebarBtn-active dark:text-dark-sidebarBtn-active"
                        : "text-sidebarBtn-inactive dark:text-dark-sidebarBtn-inactive",
                    )}
                  />
                </Link>
              );
            })}
      </div>
    </aside>
  );
};

export default Sidebar;
