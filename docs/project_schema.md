# TaskFlow Project Schema (from DB)

## 1) Hierarchy
- Team
  Space
  (optional) Folder
  List
        - Task
          - Checklist / ChecklistItems
          - Comments
          - Attachments
          - Tags
          - Assignees
          - TimeEntries

## 2) Core Entities & Relations
### Teams & Members
- Teams: quản lý nhóm
- TeamMembers: User thuộc Team + Role (phân quyền)

### Spaces / Folders / Lists
- Spaces: thuộc Team
- Folders: thuộc Space
- Lists: thuộc Folder (hoặc theo thiết kế DB bạn đang dùng)

### Tasks
- Tasks: thuộc List
- TaskAssignees: (TaskId, UserId)
- TaskTags: (TaskId, TagId)
- Comments: thuộc Task
- Attachments: thuộc Task hoặc Comment (tuỳ schema)
- Checklists: thuộc Task
- ChecklistItems: thuộc Checklist
- TimeEntries: log thời gian theo Task/User

### Chat
- Conversations: phiên chat theo User/Team
- Messages: thuộc Conversation (Role: user/model)

## 3) Business Rules (to confirm/implement)
- Status task: Todo / Doing / Done (chuẩn hoá string và filter)
- Permission:
  - TeamMembers.Role quyết định được xem/sửa task nào
  - User chỉ truy vấn task trong team mình thuộc
- Query scope:
  - Mọi truy vấn task phải lọc theo TeamId (trực tiếp hoặc suy ra qua Space/Folder/List)

## 4) Glossary
- Space/Folder/List dùng để tổ chức task theo cấu trúc dự án giống ClickUp
- Conversation/Message dùng cho chat history
