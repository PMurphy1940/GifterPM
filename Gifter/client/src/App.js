import React from "react";
import "./App.css";
import { PostProvider } from "./providers/PostProvider";
import PostList from "./providers/PostList";
import PostForm from "./providers/PostForm"

function App() {
  return (
    <div className="App">
      <PostProvider>
        <PostForm />
        <PostList />
      </PostProvider>
    </div>
  );
}

export default App;
