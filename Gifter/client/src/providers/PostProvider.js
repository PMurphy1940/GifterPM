import React, { useState } from "react";

export const PostContext = React.createContext();

export const PostProvider = (props) => {
  const [posts, setPosts] = useState([]);
  const [users, setUsers] = useState([]);

  const getAllPosts = () => {
    getToken().then((token) =>
      fetch("/api/post?comments=true", {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
        .then((res) => res.json())
        .then(setPosts)
    );
  };

  const getAllUsers = () => {
    return fetch("/api/userprofile").then((res) => res.json().then(setUsers));
  };

  const getThisUsersPosts = (id) => {};

  const getPost = (id) => {
    getToken().then((token) =>
      fetch(`/api/post/getwithcomments/${id}`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }).then((res) => res.json())
    );
  };

  const searchPosts = (q) => {
    getToken().then((token) =>
      fetch(`/api/post/search/?q=${q}`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
        .then((response) => response.json())
        .then(setPosts)
    );
  };

  const addPost = (post) => {
    getToken()
      .then((token) =>
        fetch("/api/post", {
          method: "POST",
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
          body: JSON.stringify(post),
        })
      )
      .then((response) => {
        if (response.ok) {
          return resp.json();
        }
        throw new Error("Unauthorized");
      });
  };

  const deletePost = (id) => {
    getToken()
      .then((token) =>
        fetch(`/api/post/${id}`, {
          method: "DELETE",
          headers: {
            Authorization: `Bearer ${token}`,
          },
        })
      )
      .then((response) => {
        if (response.ok) {
          return resp.json();
        }
        throw new Error("Unauthorized");
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
