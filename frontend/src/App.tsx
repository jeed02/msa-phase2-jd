import axios from "axios";
import React, { useState } from "react";
import "./App.css";

function App() {
  const [charName, setCharName] = useState<string>("");
  const [charInfo, setCharInfo] = useState<[]>([]);

  const GENSHIN_BASE_API_URL = "https://api.genshin.dev/characters/";
  return (
    <div className="App">
      <div>
        <label>Name a Genshin Character:</label>
        <br />
        <input
          type="text"
          id="char-name"
          name="char-name"
          onChange={(e) => setCharName(e.target.value)}
        />
        <br />
        <button onClick={search}>Search</button>
        {charInfo === null}(<p>Pokemon not found</p>) : (
        <div id="char-result">
          <img src={GENSHIN_BASE_API_URL + charName + "/card"} />
          <p></p>
        </div>
        )
      </div>
    </div>
  );
  function search() {
    console.log(charName);
    if (charName === undefined || charName === "") {
      return;
    }

    axios
      .get(GENSHIN_BASE_API_URL + charName?.toLowerCase())
      .then((res) => {
        setCharInfo(res.data);
        console.log(res.data.affiliation);
      })
      .catch(() => {
        setCharInfo([]);
      });

    console.log();
  }
}

export default App;
