import React, { useContext, useEffect } from "react";
import { PostContext } from "../providers/PostProvider";
import Post from "./Post";

const PostList = (props) => {
  const { posts, getAllPosts, deletePost } = useContext(PostContext);

  const handleDelete = (id) => {
    deletePost(id);
    getAllPosts();
  };

  useEffect(() => {
    getAllPosts();
  }, []);

  return (
    <div className="container">
      <div className="row justify-content-center">
        <div className="cards-column">
          {posts.map((post) => (
            <Post key={post.id} post={post} handleDelete={handleDelete} />
          ))}
        </div>
      </div>
    </div>
  );
};

export default PostList;
