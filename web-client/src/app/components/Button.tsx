"use client";

import styled from "styled-components";

const Button = styled.button<{$highlight?: boolean;}>`
  padding: 8px 24px;
  color: ${props => props.$highlight ? "white" : "black" };
  background-color: ${props => props.$highlight ? "black" : "#f2f2f2" };
  border-radius: 50px;
  font-weight: 600;
  &:hover {
    background-color: ${props => props.$highlight ? "#313131" : "#e3e3e3" };
  }
`;

export default Button;
