import React, { useEffect, useContext, useState } from "react";
import { Card, CardImg, CardBody } from "reactstrap";
import Comment from "./Comment";
import { PostContext } from "./PostProvider";
import "./post.css";

const PostForm = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [post, setPost] = useState({
    title: "",
    ImageUrl: "",
    caption: "",
    userProfileId: "",
  });
  const [search, setSearch] = useState({
      search: ""
  })
  const { users, getAllUsers, addPost, searchPosts } = useContext(PostContext);
  const [showPreview, setShowPreview] = useState(false);
  const [postUserName, setPostUserName] = useState();

  const togglePreview = () => {
    setShowPreview(!showPreview);
  };

  useEffect(() => {
    getAllUsers();
    setIsLoading(false);
  }, []);

  useEffect(() => {
    if (post.userProfileId !== "") {
      let selectedUser = users.find((user) => {
        return user.id === parseInt(post.userProfileId);
      });

      setPostUserName(selectedUser.name);
    }
  }, [post.userProfileId]);

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
  
  const handleSearchFieldChange = (event) => {
    const searchToChange = { ...search };
    searchToChange[event.target.id] = event.target.value;
    setSearch(searchToChange);
  };

  const submitNewPost = () => {
    addPost(constructNewPost());
  };

  const constructNewPost = () => {
    let newPost = {
      ...post,
      userProfileId: parseInt(post.userProfileId),
      dateCreated: new Date(),
    };
    return newPost;
  };

  return (
    <div>
      <div className="searchFields">
        <div className="input__field search">
          <input
            type="text"
            required
            onChange={handleSearchFieldChange}
            id="search"
            placeholder="Search for..."
          />
        </div>
        <button className="submit_Button" type="button" onClick={ () => searchPosts(search.search) }>
          Search Posts
        </button>
      </div>
      <h4>Add a Post</h4>
      <fieldset className="post__Form__Fields">
        <div className="input__field">
          <label htmlFor="title">Post Title</label>
          <input
            type="text"
            required
            onChange={handleFieldChange}
            id="title"
            placeholder="Post title"
          />
        </div>
        <div className="input__field">
          <label htmlFor="ImageUrl">Image Link</label>
          <input
            type="text"
            required
            onChange={handleFieldChange}
            id="ImageUrl"
            placeholder="Image Link"
          />
        </div>
        <div className="input__field">
          <label htmlFor="caption">Caption</label>
          <input
            type="text"
            onChange={handleFieldChange}
            id="caption"
            placeholder="caption"
          />
        </div>
        <div className="input__field">
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
        </div>
        <button
          className="submit_Button"
          type="button"
          disabled={isLoading}
          onClick={submitNewPost}
        >
          Submit
        </button>
        <button
          className="submit_Button"
          type="button"
          disabled={isLoading}
          onClick={togglePreview}
        >
          Show/Hide preview
        </button>
      </fieldset>
      {showPreview && (
        <div className="post_Card">
          <h4>Preview</h4>
          <Card className="m-4">
            {postUserName !== undefined && (
              <p className="text-left px-2">Posted by: {postUserName}</p>
            )}
            <CardImg src={post.ImageUrl} alt={post.title} />
            <CardBody>
              <p>{post.title !== undefined && <strong>{post.title}</strong>}</p>
              {post.caption !== undefined && <p>{post.caption}</p>}
            </CardBody>
          </Card>
        </div>
      )}
    </div>
  );
};

export default PostForm;
