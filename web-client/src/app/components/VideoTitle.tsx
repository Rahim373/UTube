"use client";

import styled from "styled-components";

const VideoTitle = styled.span<{$largeTitle?: boolean}>`
  font-size: ${props => props.$largeTitle ? '1.5rem' : '1rem'};
  font-weight: 600;
  text-overflow: ellipsis;
  overflow: hidden;
  line-height: ${props => props.$largeTitle ? '1.9rem' : '1.2rem'};
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
`;

export default VideoTitle;
