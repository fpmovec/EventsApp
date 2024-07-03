import { useNavigate, useParams } from "react-router-dom";
import styles from "./Booking.module.scss";
import { useEffect, useState } from "react";
import { WhiteButton } from "../../Components/Generic/Button/Buttons";
import { EventItemExtended } from "../../lib/DTOs/Event";
import { GetEventById } from "../../lib/Requests/GET/EventsRequests";
import { useAppSelector } from "../../lib/Redux/Hooks";
import { useForm } from "react-hook-form";
import { TextField } from "@mui/material";
import { ErrorField } from "../../Components/Generic/ErrorField/ErrorField";
import Calendar from "../../Components/Generic/Calendar/Calendar";

type FormData = {
  fullName: string;
  birthday: Date;
  personsCount: number;
  email: string;
};

const Booking = () => {
  const { eventId } = useParams();
  const stepsCount: number = 3;
  const [currentStep, setCurrentStep] = useState<number>(1);
  const [currentEvent, setCurrentEvent] = useState<EventItemExtended | null>(
    null
  );

  const [birthday, setBirthday] = useState<Date>(new Date());
  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    formState: { errors, isValid },
  } = useForm<FormData>({ mode: "onBlur" });

  const currentUser = useAppSelector((state) => state.auth.user);

  const [bookingData, setBookingData] = useState<FormData>();

  const onSubmit = (data: FormData) =>
    setBookingData({ ...data, birthday: birthday });

  useEffect(() => {
    const getEvent = async () => {
      const event = await GetEventById(eventId as string);
      setCurrentEvent(event);
    };
    getEvent();
  }, [eventId]);

  const nextStep = () => {
    if (currentStep !== stepsCount && isValid) {
      setCurrentStep((prev) => prev + 1);
    }
  };

  const prevStep = () => {
    if (currentStep !== 1) {
      setCurrentStep((prev) => prev - 1);
    }
  };

  const getStyles = () => {
    if (currentStep === 1) return styles.step1;
    if (currentStep === 2) return styles.step2;
    if (currentStep === 3) return styles.step3;
  };

  return (
    <div className={styles.main}>
      <div className={styles.content}>
        <div className={styles.title}>
          <h4>{currentEvent?.name}</h4>
        </div>
        <div className={`${styles.carousel} ${getStyles()}`}>
          <div className={styles.step}>
            <h6
              style={{
                fontFamily: "Open Sans",
                fontWeight: 300,
                textAlign: "center",
              }}
            >
              Please, fill form to book the ticket
            </h6>
            <form id="bookingForm" onSubmit={handleSubmit(onSubmit)}>
              <TextField
                label="Full Name"
                type="text"
                margin="normal"
                defaultValue={currentUser?.name}
                sx={{ width: 400 }}
                {...register("fullName", {
                  required: true,
                })}
              />
              {errors.fullName && errors.fullName.type === "required" && (
                <ErrorField data="Full name is required" />
              )}
              <TextField
                sx={{ width: 400 }}
                label="Email"
                type="text"
                margin="normal"
                defaultValue={currentUser?.email}
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
                sx={{ width: 400 }}
                label="Participants count"
                type="number"
                margin="normal"
                fullWidth
                {...register("personsCount", {
                  required: true,
                  min: 1,
                })}
              />
              {errors.personsCount &&
                errors.personsCount.type === "required" && (
                  <ErrorField data="Persons count is required" />
                )}
              {errors.personsCount && errors.personsCount.type === "min" && (
                <ErrorField data="Persons count cannot be less than 1" />
              )}
              <br />
              <Calendar
                handleValue={setBirthday}
                valueDate={birthday as Date}
                label="Birthday"
                isPastForbidden={false}
              />
            </form>
          </div>
          <div className={styles.step}>
            <h6
              style={{
                fontFamily: "Open Sans",
                fontWeight: 300,
                textAlign: "center",
                marginBottom: 15,
              }}
            >
              Please, check if data is correct
            </h6>
            <ul>
              <li>
                <span>Event name: </span>
                {currentEvent?.name}
              </li>
              <li>
                <span>Your full name: </span>
                {bookingData?.fullName}
              </li>
              <li>
                <span>Your email: </span>
                {bookingData?.email}
              </li>
              <li>
                <span>Participants count: </span>
                {bookingData?.personsCount}
              </li>
              <li>
                <span>Your birthday: </span>
                {bookingData?.birthday.toLocaleDateString()}
              </li>
            </ul>
          </div>
          <div className={styles.step}>
            <h4
              style={{
                fontFamily: "Open Sans",
                fontWeight: 300,
                textAlign: "center",
                marginBottom: 15,
              }}
            >
              Thank you for the booking!
              <br />
              Click "Next" to move to your booked tickets
            </h4>
          </div>
        </div>
        <div className={styles.buttons}>
          {currentStep === 2 && (
            <WhiteButton text="< Back" onClick={prevStep} />
          )}

          <WhiteButton
            text="Next >"
            type="submit"
            form={currentStep < 3 ? "bookingForm" : ""}
            onClick={() => {
              if (currentStep === stepsCount) navigate("/booked");
              else nextStep();
            }}
          />
        </div>
      </div>
    </div>
  );
};

export default Booking;
