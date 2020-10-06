import React, { useState } from "react";

export const PostContext = React.createContext();

export const PostProvider = (props) => {
  const [posts, setPosts] = useState([]);
  const [users, setUsers] = useState([]);

  const getAllPosts = () => {
    return fetch("/api/post?comments=true")
      .then((res) => res.json())
      .then(setPosts);
  };

  const getAllUsers = () => {
    return fetch("/api/userprofile").then((res) => res.json().then(setUsers));
  };

  const getThisUsersPosts = (id) => {};

  const getPost = (id) => {
    return fetch(`/api/post/getwithcomments/${id}`).then((res) => res.json());
  };

  const searchPosts = (q) => {
    console.log(q);
    return fetch(`/api/post/search/?q=${q}`)
      .then((response) => response.json())
      .then(setPosts);
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

  const deletePost = (id) => {
    return fetch(`/api/post/${id}`, {
      method: "DELETE",
    });
  };

  return (
    <PostContext.Provider
      value={{
        users,
        posts,
        getAllUsers,
        getAllPosts,
        addPost,
        deletePost,
        searchPosts,
        getPost,
        getThisUsersPosts,
      }}
    >
      {props.children}
    </PostContext.Provider>
  );
};
