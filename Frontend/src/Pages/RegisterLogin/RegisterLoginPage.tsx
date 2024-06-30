import { useState } from "react";
import styles from "./RegisterLoginPage.module.scss";
import { Box, Tab, Tabs } from "@mui/material";
import { TextField } from "@mui/material";
import { WhiteButton } from "../../Components/Generic/Button/Buttons";
import { useForm } from "react-hook-form";
import { ErrorField } from "../../Components/Generic/ErrorField/ErrorField";

type FormData = {
  email: string;
  password: string;
  name: string;
};

const RegisterLoginPage = () => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>({ mode: "onBlur" });

  const [tab, setTab] = useState(0);
  const [password, setPassword] = useState<string>("");
  const [isPasswordVisible, setIsPasswordVisible] = useState<boolean>(false);

  return (
    <>
      <h3 className={styles.title}>
        Log In or Register to book tickets right now
      </h3>
      <div className={styles.main}>
        <div className={styles.block}>
          <Tabs value={tab} onChange={(e, n) => setTab(n)}>
            <Tab label="Sign In" />
            <Tab label="Register" />
          </Tabs>
          <CustomTabPanel value={tab} index={0}>
            <h4
              style={{
                textAlign: "center",
                marginTop: 15,
                marginBottom: 20,
                fontWeight: 300,
              }}
            >
              Sign in
            </h4>
            <form
              onSubmit={(event) =>
                void handleSubmit(() => console.log("Form was submitted"))(
                  event
                )
              }
              className={styles.form}
            >
              <TextField
                label="Email"
                type="text"
                margin="normal"
                fullWidth
                {...register("email", {
                  required: true,
                  pattern: new RegExp(
                    /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i
                  ),
                })}
              />
              {errors.email && errors.email.type === "required" && (
                <ErrorField data="Email is required" />
              )}
              {errors.email && errors.email.type === "pattern" && (
                <ErrorField data="Email is not correct" />
              )}
              <TextField
                label="Password"
                type="password"
                margin="normal"
                fullWidth
                {...register("password", {
                  required: true,
                  minLength: 6,
                  pattern: new RegExp(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/),
                })}
                helperText="Password must contain alphabetical symbols(A-Z, a-z) and digits(0-9). Min length of the password is 6"
              />
              {errors.password && errors.password.type === "required" && (
                <ErrorField data="Password is required" />
              )}
              {errors.password && errors.password.type === "minLength" && (
                <ErrorField data="Password must have at least 6 characters" />
              )}
              {errors.password && errors.password.type === "pattern" && (
                <ErrorField data="Password does not satisfy the restrictions" />
              )}
              <div style={{ justifySelf: "flex-end", marginTop: 25 }}>
                <WhiteButton type="submit" text="Sign In" />
              </div>
            </form>
          </CustomTabPanel>
          <CustomTabPanel value={tab} index={1}>
            <h4
              style={{
                textAlign: "center",
                marginTop: 15,
                marginBottom: 20,
                fontWeight: 300,
              }}
            >
              Register
            </h4>
            <form
              onSubmit={(event) => void handleSubmit(() => {})(event)}
              className={styles.form}
            >
              <TextField
                label="Name"
                type="text"
                margin="normal"
                fullWidth
                {...register("name", {
                  required: true,
                })}
              />
              {errors.name && errors.name.type === "required" && (
                <ErrorField data="Name is required" />
              )}
              <TextField
                label="Email"
                type="text"
                margin="normal"
                fullWidth
                {...register("email", {
                  required: true,
                  pattern: new RegExp(
                    /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i
                  ),
                })}
              />
              {errors.email && errors.email.type === "required" && (
                <ErrorField data="Email is required" />
              )}
              {errors.email && errors.email.type === "pattern" && (
                <ErrorField data="Email is not correct" />
              )}
              <TextField
                label="Password"
                type="password"
                margin="normal"
                fullWidth
                {...register("password", {
                  required: true,
                  minLength: 6,
                  pattern: new RegExp(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/),
                })}
                helperText="Password must contain alphabetical symbols(A-Z, a-z) and digits(0-9). Min length of the password is 6"
              />
              {errors.password && errors.password.type === "required" && (
                <ErrorField data="Password is required" />
              )}
              {errors.password && errors.password.type === "minLength" && (
                <ErrorField data="Password must have at least 6 characters" />
              )}
              {errors.password && errors.password.type === "pattern" && (
                <ErrorField data="Password does not satisfy the restrictions" />
              )}

              <div style={{ justifySelf: "flex-end", marginTop: 25 }}>
                <WhiteButton type="submit" text="Register" />
              </div>
            </form>
          </CustomTabPanel>
        </div>
      </div>
    </>
  );
};

export default RegisterLoginPage;

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function CustomTabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && <Box sx={{ p: 3 }}>{children}</Box>}
    </div>
  );
}