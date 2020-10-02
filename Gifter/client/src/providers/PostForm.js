import React, { useEffect, useContext, useState } from "react";
import { PostContext } from "./PostProvider";

const PostForm = () => {
    const [isLoading, setIsLoading] = useState(true);
  const [post, setPost] = useState({
    title: "",
    ImageUrl: "",
    caption: "",
    userProfileId: "",
  });
  const { users, getAllUsers, addPost } = useContext(PostContext);

  useEffect(() => {
    getAllUsers();
    setIsLoading(false)
  }, []);


  const userSelect = () => {
    return (
      <>
        {users.map((user) => (
          <UserOption key={user.id} user={user} />
        ))}
      </>
    );
  };

  const UserOption = (props) => {
    return <option value={props.user.id}>{props.user.name}</option>;
  };

  const handleFieldChange = (event) => {
    const stateToChange = { ...post };
    stateToChange[event.target.id] = event.target.value;
    setPost(stateToChange);
  };
  const submitNewPost = () => {
      addPost(constructNewPost())
  }

  const constructNewPost = () => {
      let newPost = {
        ...post,
        userProfileId: parseInt(post.userProfileId),
        dateCreated: new Date()
      };
      return newPost;
  }

  return (
    <div>
      <h4>Add a Post</h4>
      <fieldset>
        <label htmlFor="title">Post Title</label>
        <input
          type="text"
          required
          onChange={handleFieldChange}
          id="title"
          placeholder="Post title"
        />
        <label htmlFor="ImageUrl">Image link</label>
        <input
          type="text"
          required
          onChange={handleFieldChange}
          id="ImageUrl"
          placeholder="ImageUrl"
        />
        <label htmlFor="caption">Caption</label>
        <input
          type="text"
          onChange={handleFieldChange}
          id="caption"
          placeholder="caption"
        />
        <select
          onChange={handleFieldChange}
          id="userProfileId"
          placeholder="userProfileId"
          required
        >
          <option htmlFor="userProfileId" value="0">
            Please tell us who you are. We trust you.
          </option>
          {userSelect()}
        </select>
        <button
              type="button"
              disabled={isLoading}
              onClick={submitNewPost}
            >Submit</button>
      </fieldset>
    </div>
  );
};

export default PostForm;
