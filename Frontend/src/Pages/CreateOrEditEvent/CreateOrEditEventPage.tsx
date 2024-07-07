import { useNavigate, useParams } from "react-router-dom";
import styles from "./CreateOrEditEventPage.module.scss";
import { useEffect, useState } from "react";
import { EventDTO, EventItemExtended } from "../../lib/Models/Event";
import { GetEventById } from "../../lib/Requests/GET/EventsRequests";
import { useForm } from "react-hook-form";
import { TextField } from "@mui/material";
import { ErrorField } from "../../Components/Generic/ErrorField/ErrorField";
import Selector from "../../Components/Generic/Select/Selector";
import Calendar from "../../Components/Generic/Calendar/Calendar";
import { BlueButton } from "../../Components/Generic/Button/Buttons";
import { useAppSelector } from "../../lib/Redux/Hooks";
import { CreateEvent } from "../../lib/Requests/POST/Event";

const CreateOrEditEventPage = () => {
  const { eventId } = useParams();

  const [currentEvent, setCurrentEvent] = useState<EventItemExtended | null>(
    null
  );

  const [place, setPlace] = useState<string>(currentEvent?.place as string);
  const [date, setDate] = useState<Date>(currentEvent?.date as Date);
  const [category, setCategory] = useState<string>(
    currentEvent?.category.name as string
  );
  const [file, setFile] = useState<File>();
  const token = useAppSelector((state) => state.auth.tokens).mainToken;

  const [isReady, setIsReady] = useState<boolean>(eventId === undefined);

  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<EventDTO>({
    mode: "onBlur",
  });

  const onSubmit = (data: EventDTO) => {
    const editedEvent: EventDTO = {
      ...data,
      place: place,
      dateTime: date ?? new Date(),
      categoryName: category,
    };
  };

  const onCreate = (data: EventDTO) => {
    const editedEvent: EventDTO = {
      ...data,
      place: place,
      dateTime: date as Date,
      categoryName: category as string,
    };

    const create = async () => {
      await CreateEvent(editedEvent, file!, token);
    };

    create();
    navigate("/");
  };

  useEffect(() => {
    const getEvent = async () => {
      const e = await GetEventById(eventId as string);
      setCurrentEvent(e);
    };

    if (eventId !== undefined) {
      getEvent();
      setIsReady(true);
    }
  }, [eventId]);
  console.log(currentEvent);
  return (
    <>
      <div className={styles.title}>
        <h3>{eventId ? "Edit" : "Create"} event</h3>
      </div>
      {isReady ? (
        <>
          <div className={styles.main}>
            <form id="editEvent" onSubmit={handleSubmit(onCreate)}>
              <TextField
                label="Event name"
                type="text"
                margin="normal"
                sx={{ width: 400 }}
                {...register("name", {
                  required: true,
                  minLength: 3,
                })}
                defaultValue={currentEvent?.name}
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
                defaultValue={currentEvent?.description}
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
                defaultValue={currentEvent?.maxParticipantsCount}
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
                defaultValue={currentEvent?.price}
                sx={{ width: 400 }}
                {...register("price", {
                  required: true,
                  min: 0,
                })}
              />
              {errors.price && errors.price.type === "required" && (
                <ErrorField data="This field is required" />
              )}
              {errors.price && errors.price.type === "min" && (
                <ErrorField data="Price cannot be less than 0" />
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
                  source={["Minsk", "Moscow", "Mogilev"]}
                  handleValue={(v) => setPlace(v)}
                  isRequired={true}
                  fullWidth={true}
                  defaultValue={currentEvent?.place}
                />
                <Selector
                  label="Category"
                  value={category ?? ""}
                  source={["Festival", "Concert", "Conference"]}
                  handleValue={(v) => setCategory(v)}
                  defaultValue={currentEvent?.category.name}
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
