import React from "react";

const Comment = (props) => {
  return <p className="aComment">{props.comment.message}</p>;
};

export default Comment;
