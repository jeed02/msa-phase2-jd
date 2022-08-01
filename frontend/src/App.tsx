import { Grid, IconButton, TextField, Paper, Rating } from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import axios from "axios";
import React, { useState } from "react";
import "./App.css";

interface Character {
  name: string;
  vision: string;
  weapon: string;
  nation: string;
  affiliation: string;
  rarity: number;
  constellation: string;
  birthday: string;
  description: string;
}

function App() {
  const [charName, setCharName] = useState<string>("");
  const [charInput, setCharInput] = useState<string>("");
  const [charInfo, setCharInfo] = useState<Character | null>(null);

  const GENSHIN_BASE_API_URL = "https://api.genshin.dev/characters/";
  return (
    <div className="main">
      <h1 className="title">Genshin Impact Search</h1>
      <div>
        <TextField
          id="search-bar"
          className="text"
          value={charInput}
          onChange={(prop: any) => {
            let name = prop.target.value;
            setCharInput(name);
            setCharName(prop.target.value.replace(/\s+/g, "-").toLowerCase());
          }}
          label="Enter a Character Name..."
          variant="outlined"
          placeholder="Search..."
          size="small"
          sx={{
            "& .MuiInputLabel-root": { color: "white" }, //styles the label
            "& .MuiOutlinedInput-root": {
              "& > fieldset": { borderColor: "white" },
            },
            "& .MuiOutlinedInput-input": { color: "white" },
          }}
        />
        <IconButton
          aria-label="search"
          onClick={() => {
            search();
          }}
        >
          <SearchIcon style={{ fill: "white" }} />
        </IconButton>
        {charInfo === null ? (
          <p className="not-found">Character not found</p>
        ) : (
          <>
            <div className="char-result">
              <Paper sx={{ backgroundColor: getBackgroundColor() }}>
                <Grid container columnSpacing={{ xs: 1, sm: 2, md: 3 }}>
                  <Grid item md={3}>
                    <img
                      src={GENSHIN_BASE_API_URL + charName + "/gacha-card"}
                    />
                  </Grid>
                  <Grid item md={9}>
                    <div className="char-items">
                      <Grid container>
                        <Grid item md={6}>
                          <h1>{charInfo.name}</h1>
                          <p className="constellation">
                            {charInfo.constellation}
                          </p>
                          <Rating
                            name="read-only"
                            value={charInfo.rarity}
                            readOnly
                          />
                        </Grid>
                        <Grid item md={6}>
                          <img
                            src={getElement(charInfo.vision)}
                            alt=""
                            style={{
                              height: "50px",
                              margin: "40px auto",
                              float: "right",
                              paddingRight: "2em",
                            }}
                          />
                        </Grid>
                      </Grid>

                      <p>{charInfo.description}</p>
                      <table className="char-info">
                        <tr>
                          <td className="char-info-header">Weapon:</td>
                          <td>{charInfo.weapon}</td>
                        </tr>
                        <tr>
                          <td className="char-info-header">Vision:</td>
                          <td>{charInfo.vision}</td>
                        </tr>
                        <tr>
                          <td className="char-info-header">Nation:</td>
                          <td>{charInfo.nation}</td>
                        </tr>
                        <tr>
                          <td className="char-info-header">Affiliation:</td>
                          <td>{charInfo.affiliation}</td>
                        </tr>
                      </table>
                    </div>
                  </Grid>
                </Grid>
              </Paper>
            </div>
          </>
        )}
      </div>
    </div>
  );

  function search() {
    console.log(charName);
    if (charName === undefined || charName === "") {
      return;
    } else {
      setCharName(charName.replace(/\s+/g, "-").toLowerCase());
    }

    axios
      .get(GENSHIN_BASE_API_URL + charName?.toLowerCase())
      .then((res) => {
        let info = JSON.parse(JSON.stringify(res.data));
        setCharInfo(info);
      })
      .catch(() => {
        setCharInfo(null);
      });
  }

  function getBackgroundColor() {
    let backColor = "#2d325a";
    return backColor;
  }

  function getElement(vision: string) {
    switch (vision) {
      case "Anemo":
        return process.env.PUBLIC_URL + "/images/Element_Anemo.png";
      case "Cryo":
        return process.env.PUBLIC_URL + "/images/Element_Cryo.png";
      case "Electro":
        return process.env.PUBLIC_URL + "/images/Element_Electro.png";
      case "Geo":
        return process.env.PUBLIC_URL + "/images/Element_Geo.png";
      case "Hydro":
        return process.env.PUBLIC_URL + "/images/Element_Hydro.png";
      case "Cryo":
        return process.env.PUBLIC_URL + "/images/Element_Pyro.png";
    }
  }
}

export default App;
