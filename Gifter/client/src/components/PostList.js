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
        <h5 className="Center_Me" >We're sorry, there are no posts to display</h5>
          {posts.length === 0 ? (
            <h5 className="Center_Me" >We're sorry, there are no posts to display</h5>
          ) : (
            posts.map((post) => (
              <Post key={post.id} post={post} handleDelete={handleDelete} />
            ))
          )}
        </div>
      </div>
    </div>
  );
};

export default PostList;
