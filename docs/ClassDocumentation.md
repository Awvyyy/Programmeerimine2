# Class documentation

## Category
| Property | Type | Description |
|---|---|---|
| Id | int | Primary key |
| Name | string(100) | Category name |
| Description | string(500) | Optional text |
| MediaItems | IList<MediaItem> | One category has many media items |

## Person
| Property | Type | Description |
|---|---|---|
| Id | int | Primary key |
| FullName | string(150) | Borrower/reviewer name |
| Email | string(200) | Optional email |
| PhoneNumber | string(50) | Optional phone |
| Loans | IList<Loan> | Borrowing history |
| Reviews | IList<Review> | Reviews written by this person |

## MediaItem
| Property | Type | Description |
|---|---|---|
| Id | int | Primary key |
| Title | string(200) | Media title |
| AuthorOrCreator | string(150) | Author, director, artist or studio |
| MediaType | enum | Book, Movie, Game, Music, Other |
| ReleaseDate | DateTime? | Optional release date |
| Price | decimal | Estimated value |
| IsAvailable | bool | Availability status |
| CategoryId | int | Category foreign key |
| Category | Category | Navigation property |
| Loans | IList<Loan> | Loans for this item |
| Reviews | IList<Review> | Reviews for this item |

## Loan
| Property | Type | Description |
|---|---|---|
| Id | int | Primary key |
| MediaItemId | int | Media item foreign key |
| BorrowerId | int | Person foreign key |
| BorrowedAt | DateTime | Borrowing date |
| DueAt | DateTime | Due date |
| ReturnedAt | DateTime? | Return date |
| Notes | string(500) | Optional notes |

## Review
| Property | Type | Description |
|---|---|---|
| Id | int | Primary key |
| MediaItemId | int | Media item foreign key |
| ReviewerId | int | Person foreign key |
| Rating | int | 1-5 rating |
| Comment | string(1000) | Review text |
| CreatedAt | DateTime | Review date |
