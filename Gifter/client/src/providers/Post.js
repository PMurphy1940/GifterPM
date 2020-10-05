import React from "react";
import { Card, CardImg, CardBody, Button } from "reactstrap";
import Comment from "./Comment"

const Post = (props) => {

console.log(props)
  return (
    <div className="post_Card">
      <Card className="m-4">
        <p className="text-left px-2">Posted by: {props.post.userProfile.name}</p>
        <CardImg top src={props.post.imageUrl} alt={props.post.title} />
        <CardBody>
          <p>
            <strong>{props.post.title}</strong>
          </p>
          <p>{props.post.caption}</p>
        </CardBody>
        <Button variant="primary" onClick={ () => props.handleDelete(props.post.id)}>Delete</Button>
      </Card>
      <div className="comment_Container">
      {props.post.comments.map((comment) => 
        <Comment key={comment.id} comment={comment} />
      )}
      </div>
    </div>
  );
};

export default Post;
