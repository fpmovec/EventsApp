import type {} from "@mui/x-date-pickers/themeAugmentation";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import "./Calendar.module.scss";
import dayjs from "dayjs";

interface Props {
  handleValue: (v: Date) => void;
  valueDate: Date;
}

const Calendar = ({ handleValue, valueDate }: Props) => {
  return (
    <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale="de">
        <DatePicker
          label="Select date"
          disablePast={true}
          onChange={(v) => handleValue(v?.toDate() ?? new Date())}
          slotProps={{
            actionBar: { actions: ["today"] },
          }}
          value={dayjs(valueDate)}
          closeOnSelect={true}
        />
    </LocalizationProvider>
  );
};

export default Calendar;
