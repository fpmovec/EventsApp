import styles from "./Sign.module.scss";
import HeaderLink from "../HeaderLinks/Link";
import { useAppSelector } from "../../../lib/Redux/Hooks";
import { useDispatch } from "react-redux";
import { signOut } from "../../../lib/Redux/Slices";
import { Logout } from "../../../lib/Requests/POST/Auth";
import { Menu, MenuItem } from "@mui/material";
import { KeyboardArrowDown, KeyboardArrowUp } from "@mui/icons-material";
import { Notifications } from "@mui/icons-material";
import React from "react";
import { useNavigate } from "react-router-dom";

const Sign = () => {
  const user = useAppSelector((state) => state.auth.user);
  const isAuthenticated = useAppSelector((state) => state.auth.isAuthenticated);
  const token = useAppSelector((state) => state.auth.tokens.mainToken);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const onSignOut = () => {
    const signO = async () => {
      await Logout(token);
      dispatch(signOut());
    };

    signO();
    navigate("/");
  };

  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);
  const handleClick = (event: React.MouseEvent<HTMLDivElement>) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <div className={styles.block}>
      {!isAuthenticated ? (
        <>
          <HeaderLink
            text={isAuthenticated ? `Hello, ${user?.email}` : "Sign In"}
            path="/login"
          />
        </>
      ) : (
        <div className={styles.profileBlock}>
          <div className={styles.notification} onClick={() => navigate('/notifications')}>
            <Notifications color="primary" fontSize="medium"/>
          </div>
          <div
            className={styles.expanderHeader}
            onClick={(e) => handleClick(e)}
          >
            <div className={styles.expanderTitle}>Profile</div>
            {open ? (
              <KeyboardArrowUp fontSize="small" />
            ) : (
              <KeyboardArrowDown fontSize="small" />
            )}
          </div>
          <Menu
            id="basic-menu"
            anchorEl={anchorEl}
            open={open}
            onClose={handleClose}
            disableScrollLock={true}
            PaperProps={{
              style: {
                width: "20ch",
              },
            }}
          >
            {user?.isAdmin && (
              <MenuItem
                onClick={() => {
                  navigate(`/event/edit`);
                  handleClose();
                }}
                divider={true}
                sx={{ justifyContent: "center" }}
              >
                Create event
              </MenuItem>
            )}
            <MenuItem
              onClick={() => {
                navigate("/booked");
                handleClose();
              }}
              divider={true}
              sx={{ justifyContent: "center" }}
            >
              My tickets
            </MenuItem>
            <MenuItem
              onClick={() => {
                onSignOut();
                handleClose();
              }}
              sx={{ justifyContent: "center" }}
            >
              Logout
            </MenuItem>
          </Menu>
        </div>
      )}
    </div>
  );
};

export default Sign;
