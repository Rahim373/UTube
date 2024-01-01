"use client";

import styled from "styled-components";

const VideoTitle = styled.span`
  font-size: 0.9rem;
  font-weight: 600;
  text-overflow: ellipsis;
  overflow: hidden;
  line-height: 1.2rem;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
`;

export default VideoTitle;
