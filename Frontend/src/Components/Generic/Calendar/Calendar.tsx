import type {} from "@mui/x-date-pickers/themeAugmentation";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import "./Calendar.module.scss";

interface Props {
  handleValue: (v: Date) => void;
}

const Calendar = ({ handleValue }: Props) => {
  return (
    <LocalizationProvider dateAdapter={AdapterDayjs}>
        <DatePicker
          label="Select date"
          disablePast={true}
          onChange={(v) => handleValue(v?.toDate() ?? new Date())}
          slotProps={{
            actionBar: { actions: ["today"] },
          }}
          closeOnSelect={true}
        />
    </LocalizationProvider>
  );
};

export default Calendar;
