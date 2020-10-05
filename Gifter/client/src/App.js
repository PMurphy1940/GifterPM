import React from "react";
import "./App.css";
import { PostProvider } from "./providers/PostProvider";
import PostList from "./providers/PostList";
import PostForm from "./providers/PostForm";

function App() {
  return (
    <div className="App">
      <PostProvider>
        <div className="main_Container">
          <div className="form">
            <PostForm />
          </div>
          <div className="Post_Scroller">
            <PostList />
          </div>
        </div>
      </PostProvider>
    </div>
  );
}

export default App;
