import { useNavigate, useParams } from "react-router-dom";
import styles from "./CreateOrEditEventPage.module.scss";
import { useEffect, useState } from "react";
import { EventDTO } from "../../lib/Models/Event";
import { GetEventById } from "../../lib/Requests/GET/EventsRequests";
import { useForm } from "react-hook-form";
import { TextField } from "@mui/material";
import { ErrorField } from "../../Components/Generic/ErrorField/ErrorField";
import Selector from "../../Components/Generic/Select/Selector";
import Calendar from "../../Components/Generic/Calendar/Calendar";
import { BlueButton } from "../../Components/Generic/Button/Buttons";
import { useAppSelector } from "../../lib/Redux/Hooks";
import { CreateEvent, UpdateEvent } from "../../lib/Requests/POST/Event";
import { cities } from "../../lib/Constants";

const CreateOrEditEventPage = () => {
  const { eventId } = useParams();

  const [place, setPlace] = useState<string>("");
  const [date, setDate] = useState<Date>(new Date());

  const [file, setFile] = useState<File>();
  const token = useAppSelector((state) => state.auth.tokens).mainToken;

  const [isReady, setIsReady] = useState<boolean>(eventId === undefined);

  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm<EventDTO>({
    mode: "onBlur",
  });

  const onEdit = (data: EventDTO) => {
    const editedEvent: EventDTO = {
      ...data,
      place: place,
      dateTime: date as Date,
    };

    const update = async () => {
      await UpdateEvent(eventId as string, editedEvent, file!, token);
    };

    update();
    navigate("/");
  };

  const onCreate = (data: EventDTO) => {
    const editedEvent: EventDTO = {
      ...data,
      place: place,
      dateTime: date as Date,
    };

    const create = async () => {
      await CreateEvent(editedEvent, file!, token);
    };

    create();
    navigate("/");
  };

  useEffect(() => {
    if (eventId !== undefined) {
      GetEventById(eventId as string).then((e) => {
        setPlace(e.place);
        setDate(e.date);
        setValue("name", e.name);
        setValue("description", e.description);
        setValue("maxParticipantsCount", e.maxParticipantsCount);
        setValue("categoryName", e.category.name);
        setValue("price", e.price);
        setValue("dateTime", e.date);
      });
      setIsReady(true);
    }
  }, [eventId]);

  return (
    <>
      <div className={styles.title}>
        <h3>{eventId ? "Edit" : "Create"} event</h3>
      </div>
      {isReady ? (
        <>
          <div className={styles.main}>
            <form
              id="editEvent"
              onSubmit={handleSubmit(eventId === undefined ? onCreate : onEdit)}
            >
              <TextField
                label="Event name"
                type="text"
                margin="normal"
                sx={{ width: 400 }}
                {...register("name", {
                  required: true,
                  minLength: 3,
                })}
              />
              {errors.name && errors.name.type === "required" && (
                <ErrorField data="Event name is required" />
              )}
              {errors.name && errors.name.type === "minLength" && (
                <ErrorField data="Event name must contain at least 3 characters" />
              )}
              <TextField
                label="Description"
                type="text"
                margin="normal"
                sx={{ width: 400 }}
                {...register("description", {
                  required: true,
                  minLength: 10,
                })}
                multiline
              />
              {errors.description && errors.description.type === "required" && (
                <ErrorField data="Description is required" />
              )}
              {errors.description &&
                errors.description.type === "minLength" && (
                  <ErrorField data="Description must contain at least 10 characters" />
                )}
              <TextField
                label="Max participants count"
                type="number"
                margin="normal"
                sx={{ width: 400 }}
                {...register("maxParticipantsCount", {
                  required: true,
                  min: 1,
                })}
              />
              {errors.maxParticipantsCount &&
                errors.maxParticipantsCount.type === "required" && (
                  <ErrorField data="This field is required" />
                )}
              {errors.maxParticipantsCount &&
                errors.maxParticipantsCount.type === "min" && (
                  <ErrorField data="Value cannot be less than 1" />
                )}
              <TextField
                label="Price"
                type="number"
                margin="normal"
                sx={{ width: 400 }}
                {...register("price", {
                  required: true,
                  min: 0,
                  max: 5000,
                })}
              />
              {errors.price && errors.price.type === "required" && (
                <ErrorField data="This field is required" />
              )}
              {errors.price && errors.price.type === "min" && (
                <ErrorField data="Value cannot be less than 0" />
              )}
              {errors.price && errors.price.type === "max" && (
                <ErrorField data="Value cannot be greater than 5000" />
              )}
              <TextField
                label="Category"
                type="text"
                margin="normal"
                sx={{ width: 400 }}
                {...register("categoryName", {
                  required: true,
                  minLength: 3,
                })}
              />
              {errors.categoryName &&
                errors.categoryName.type === "required" && (
                  <ErrorField data="This field is required" />
                )}
              {errors.categoryName &&
                errors.categoryName.type === "minLength" && (
                  <ErrorField data="Name must contain at least 3 characters" />
                )}
              <div
                style={{
                  width: 400,
                  display: "flex",
                  flexDirection: "row",
                  justifyContent: "space-evenly",
                  marginTop: 10,
                  marginBottom: 20,
                }}
              >
                <Selector
                  label="City"
                  value={place}
                  source={cities}
                  handleValue={(v) => setPlace(v)}
                  isRequired={true}
                  fullWidth={true}
                />
              </div>
              <Calendar valueDate={date as Date} handleValue={setDate} />
              <br />
              <input
                type="file"
                onChange={(e) => setFile(e.target.files![0])}
              />
            </form>
            <div style={{ marginLeft: 300, marginTop: 35 }}>
              <BlueButton type="submit" form="editEvent" text="Confirm" />
            </div>
          </div>
        </>
      ) : (
        <h4 style={{ marginTop: 35, textAlign: "center" }}>Loading...</h4>
      )}
    </>
  );
};

export default CreateOrEditEventPage;
