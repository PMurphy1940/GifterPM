import React, { useState } from "react";

export const PostContext = React.createContext();

export const PostProvider = (props) => {
  const [posts, setPosts] = useState([]);
  const [users, setUsers] = useState([]);

  const getAllPosts = () => {
    return fetch("/api/post")
      .then((res) => res.json())
      .then(setPosts);
  };

  const getAllUsers = () => {
    return fetch("/api/userprofile").then((res) => res.json().then(setUsers));
  };

  const addPost = (post) => {
    return fetch("/api/post", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(post),
    });
  };

  return (
    <PostContext.Provider
      value={{ users, posts, getAllUsers, getAllPosts, addPost }}
    >
      {props.children}
    </PostContext.Provider>
  );
};
