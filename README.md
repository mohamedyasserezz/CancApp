# CancApp Admin Dashboard

A comprehensive React-based admin dashboard for managing the CancApp platform. This dashboard provides tools for user management, content moderation, profile verification, and system analytics.

## Features

### 🔐 Authentication
- Secure admin login with JWT token authentication
- Automatic session management with refresh tokens
- Protected routes and automatic logout on session expiry

### 📊 Dashboard Overview
- Real-time user statistics with visual charts
- Breakdown by user types (Doctors, Patients, Pharmacists, Volunteers, Psychiatrists)
- Interactive pie charts and bar graphs using Recharts

### 👥 User Management
- Enable/disable user accounts
- Add warnings to user accounts
- Real-time user status management

### ✅ Profile Verification
- Review pending professional profile verifications
- View profile images and professional licenses
- Approve or reject profile submissions
- Support for Doctors, Pharmacists, and Psychiatrists

### 🛡️ Content Moderation
- View and manage reported posts
- Review reported comments
- Monitor top posts by engagement metrics
- Content detail dialogs for better review

### 🎨 Modern UI/UX
- Responsive Material-UI design
- Mobile-friendly interface
- Custom CancApp branding
- Loading states and error handling
- Smooth animations and transitions

## Prerequisites

Before running this application, make sure you have the following installed:

- **Node.js** (version 16 or higher)
- **npm** (comes with Node.js) or **yarn**

## Installation

1. **Clone or download the project files**

2. **Install dependencies**
   ```bash
   npm install
   ```
   or if you prefer yarn:
   ```bash
   yarn install
   ```

3. **Start the development server**
   ```bash
   npm start
   ```
   or with yarn:
   ```bash
   yarn start
   ```

4. **Open your browser**
   The application will automatically open at `http://localhost:3000`

## Configuration

### API Base URL
The application is configured to connect to the CancApp backend at:
```
http://cancapp.runasp.net/api
```

If you need to change the API endpoint, modify the `BASE_URL` in `src/services/api.js`.

### Environment Variables
Create a `.env` file in the root directory if you need to customize settings:

```env
REACT_APP_API_BASE_URL=http://cancapp.runasp.net/api
REACT_APP_APP_NAME=CancApp Dashboard
```

## Usage

### Login
1. Navigate to the login page
2. Enter your admin credentials (email and password)
3. The system will authenticate and redirect to the dashboard

### Dashboard Navigation
- **Dashboard**: Overview with user statistics and charts
- **User Management**: Enable/disable users and add warnings
- **Profile Verification**: Review and approve professional profiles
- **Content Moderation**: Manage reported content and view top posts

### User Management
1. Go to "User Management" section
2. Enter the User ID of the user you want to manage
3. Choose the appropriate action (Disable, Enable, or Add Warning)
4. Confirm the action

### Profile Verification
1. Navigate to "Profile Verification"
2. Review pending profile submissions
3. Click "View Details" to see full profile information
4. Approve or reject the profile

### Content Moderation
1. Go to "Content Moderation"
2. Switch between tabs to view:
   - Reported Comments
   - Reported Posts
   - Top Posts
3. Click "View Details" to see full content
4. Take appropriate moderation actions

## Project Structure

```
src/
├── components/          # Reusable UI components
│   └── Layout/         # Layout components
├── pages/              # Page components
├── services/           # API services
├── store/              # Redux store and slices
│   └── slices/         # Redux slices
├── App.js              # Main app component
├── index.js            # App entry point
└── theme.js            # Material-UI theme configuration
```

## Dependencies

### Core Dependencies
- **React** (18.2.0) - UI library
- **React Router** (6.20.1) - Client-side routing
- **Redux Toolkit** (1.9.7) - State management
- **Material-UI** (5.14.20) - UI component library
- **Axios** (1.6.2) - HTTP client
- **Recharts** (2.8.0) - Chart library

### Development Dependencies
- **React Scripts** (5.0.1) - Development and build tools
- **Testing Library** - Component testing

## API Endpoints

The dashboard integrates with the following backend endpoints:

### Authentication
- `POST /api/Dashboard/login` - Admin login

### Dashboard
- `GET /api/Dashboard/UserCharts` - Get user statistics
- `GET /api/Dashboard/UnCompletedProfile` - Get uncompleted profiles
- `POST /api/Dashboard/ConfirmCompleteProfile` - Approve profile
- `POST /api/Dashboard/FailCompleteProfile` - Reject profile

### User Management
- `POST /api/Dashboard/disable` - Disable user
- `POST /api/Dashboard/enable` - Enable user
- `POST /api/Dashboard/warning` - Add warning

### Content Moderation
- `GET /api/Dashboard/reported-comments` - Get reported comments
- `GET /api/Dashboard/reported-posts` - Get reported posts
- `GET /api/Dashboard/top-posts` - Get top posts

## Troubleshooting

### Common Issues

1. **CORS Errors**
   - Ensure the backend allows requests from `http://localhost:3000`
   - Check if the API base URL is correct

2. **Authentication Issues**
   - Verify admin credentials
   - Check if the JWT token is being sent correctly
   - Clear browser storage if needed

3. **Build Errors**
   - Delete `node_modules` and `package-lock.json`
   - Run `npm install` again
   - Clear npm cache: `npm cache clean --force`

4. **Port Already in Use**
   - The app will automatically suggest using a different port
   - Or manually kill the process using the port

### Performance Tips

- The app uses React.memo and useMemo for performance optimization
- API responses are cached in Redux store
- Images are lazy-loaded for better performance
- Charts are responsive and optimized

## Browser Support

- Chrome (recommended)
- Firefox
- Safari
- Edge

## Security Features

- JWT token authentication
- Automatic token refresh
- Protected routes
- Input validation
- XSS protection
- CSRF protection (handled by backend)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is part of the CancApp platform and is proprietary software.

## Support

For technical support or questions, please contact the development team.

---

**Note**: This dashboard requires access to the CancApp backend API. Make sure you have the correct API credentials and permissions before using the dashboard. 