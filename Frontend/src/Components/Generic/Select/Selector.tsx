import { Select, FormControl, MenuItem, InputLabel } from "@mui/material";

interface Props {
  label: string;
  source: string[];
  value: string;
  handleValue: (i: string) => void;
}

const Selector = ({ label, value, source, handleValue }: Props) => {
  return (
    <>
      <FormControl style={{width: 170}}>
        <InputLabel id={label}>{label}</InputLabel>
        <Select
          labelId={label}
          label={label}
          onChange={(e) => handleValue(e.target.value as string)}
          value={value}
          MenuProps={{ disableScrollLock: true }}
          fullWidth={false}
        >
          {source.map((item, index) => {
            return (
              <MenuItem key={index} value={item}>
                {item}
              </MenuItem>
            );
          })}
        </Select>
      </FormControl>
    </>
  );
};

export default Selector;
