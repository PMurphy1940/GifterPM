import React from "react";
import { Card, CardImg, CardBody } from "reactstrap";
import Comment from "./Comment"

const Post = ({ post }) => {


  return (
    <div className="post_Card">
      <Card className="m-4">
        <p className="text-left px-2">Posted by: {post.userProfile.name}</p>
        <CardImg top src={post.imageUrl} alt={post.title} />
        <CardBody>
          <p>
            <strong>{post.title}</strong>
          </p>
          <p>{post.caption}</p>
        </CardBody>
      </Card>
      <div className="comment_Container">
      {post.comments.map((comment) => 
        <Comment key={comment.id} comment={comment} />
      )}
      </div>

    </div>
  );
};

export default Post;
