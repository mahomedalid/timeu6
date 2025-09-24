# TimeU6 - U6 Soccer Team Management App

## Project Overview
A Blazor WebAssembly offline application for managing U6 soccer team rosters, tracking playing time, and facilitating easy player substitutions during matches.

## Current Technology Stack
- **Framework**: Blazor WebAssembly (.NET 9.0)
- **UI**: Bootstrap 5 with custom CSS
- **Storage**: Local Storage (Browser-based for offline functionality)
- **State Management**: Blazor component state with services

## Core Features

### 1. Team Management
- **Player Roster Configuration**
  - Add/Edit/Remove players
  - Player details (name, jersey number, position preferences)
  - Player photos (optional)
  - Medical notes/allergies
  - Parent contact information
  - Player availability preferences

### 2. Match Management
- **Match Setup**
  - Create new match
  - Set match duration and period structure
  - Configure substitution rules (e.g., every X minutes)
  - Set target playing time per player
  - Mark player attendance (present/absent/late)

- **Live Match Interface**
  - Real-time match timer with period tracking
  - Current lineup display (field positions)
  - Bench players display
  - Drag-and-drop player substitutions
  - Quick swap buttons
  - Emergency substitution alerts

### 3. Playing Time Tracking
- **Time Management**
  - Individual player timers
  - Automatic time tracking when on field
  - Playing time balance indicators
  - Visual alerts for rotation suggestions
  - Flexible override controls

- **Analytics & Reports**
  - Match summary reports
  - Season playing time statistics
  - Fairness metrics (time distribution)
  - Player performance trends

### 4. User Experience Features
- **Intuitive Controls**
  - Large touch-friendly buttons
  - Color-coded player status
  - Audio/vibration alerts for substitutions
  - Quick access toolbar
  - Undo/redo functionality

- **Offline Functionality**
  - Complete offline operation
  - Local data persistence
  - Export match data
  - Backup/restore team data

## Technical Architecture

### Data Models
```
Team
├── Players[]
├── Matches[]
├── Settings
└── SeasonStatistics

Player
├── PersonalInfo (name, number, position)
├── ContactInfo (parent details)
├── MatchHistory[]
├── PlayingTimeStats
└── Preferences

Match
├── MatchInfo (date, opponent, venue)
├── Attendance[]
├── Periods[]
├── Substitutions[]
├── PlayingTime[]
└── Notes

MatchPeriod
├── StartTime
├── EndTime
├── ActivePlayers[]
└── Events[]
```

### Services Architecture
```
TeamService
├── PlayerManagement
├── DataPersistence
└── Export/Import

MatchService
├── MatchTimer
├── PlayerTracking
├── SubstitutionLogic
└── TimeCalculation

NotificationService
├── AudioAlerts
├── VisualNotifications
└── RotationReminders

StorageService
├── LocalStorage
├── DataSerialization
└── BackupManagement
```

## Implementation Phases

### Phase 1: Foundation (Week 1-2)
- **Data Models & Services Setup**
  - Create core data models
  - Implement local storage service
  - Set up dependency injection
  - Create team and player management services

- **Basic UI Framework**
  - Update navigation menu
  - Create responsive layout system
  - Implement theme and styling
  - Set up component library structure

### Phase 2: Team Management (Week 2-3)
- **Player Management**
  - Player CRUD operations
  - Player profile pages
  - Team roster overview
  - Import/export player data

- **Team Configuration**
  - Team settings page
  - Playing time rules configuration
  - Match format settings
  - Notification preferences

### Phase 3: Match Setup (Week 3-4)
- **Match Creation**
  - New match wizard
  - Attendance tracking
  - Starting lineup selection
  - Match settings configuration

- **Pre-Match Validation**
  - Ensure minimum players present
  - Validate formation rules
  - Check equipment assignments
  - Generate match timeline

### Phase 4: Live Match Interface (Week 4-6)
- **Real-time Match Control**
  - Match timer with period management
  - Live player status display
  - Drag-and-drop substitutions
  - Quick action buttons

- **Playing Time Tracking**
  - Individual player timers
  - Automatic time calculation
  - Balance monitoring
  - Rotation suggestions

### Phase 5: Advanced Features (Week 6-8)
- **Smart Notifications**
  - Audio alerts for substitutions
  - Visual rotation reminders
  - Customizable notification rules
  - Emergency situation handling

- **Analytics Dashboard**
  - Match summary reports
  - Season statistics
  - Playing time fairness metrics
  - Export functionality

### Phase 6: Polish & Testing (Week 8-9)
- **User Experience Enhancements**
  - Performance optimization
  - Accessibility improvements
  - Mobile responsiveness
  - Error handling

- **Quality Assurance**
  - Comprehensive testing
  - Data validation
  - Edge case handling
  - User acceptance testing

## Key User Flows

### 1. Setting Up a New Team
1. Create team profile
2. Add players with details
3. Configure playing time rules
4. Set up match format preferences
5. Test notification settings

### 2. Preparing for a Match
1. Create new match entry
2. Mark player attendance
3. Set match-specific rules
4. Select starting lineup
5. Review rotation strategy

### 3. Managing Live Match
1. Start match timer
2. Monitor playing time balance
3. Execute drag-and-drop substitutions
4. Respond to rotation alerts
5. Handle emergency situations
6. Complete match summary

### 4. Post-Match Analysis
1. Review match statistics
2. Analyze playing time distribution
3. Export match report
4. Update season statistics
5. Plan for next match

## Technical Considerations

### Offline Storage Strategy
- Use browser LocalStorage for persistence
- Implement JSON serialization for data
- Create backup/restore functionality
- Handle storage quota limitations

### Performance Optimization
- Lazy loading for large rosters
- Efficient timer management
- Optimized drag-and-drop operations
- Minimal re-rendering strategies

### Mobile Responsiveness
- Touch-friendly interface design
- Landscape/portrait orientation support
- Large, accessible buttons
- Gesture-based controls

### Data Security & Privacy
- Local-only data storage
- No external data transmission
- Parental consent considerations
- Data anonymization options

## Future Enhancement Opportunities
- Multi-team management
- Statistics comparison with other teams
- Integration with league systems
- Advanced analytics and AI recommendations
- Photo/video integration for match highlights
- Communication features for parents
- Equipment tracking functionality

## Success Metrics
- Ease of use during live matches
- Accurate playing time distribution
- User satisfaction with substitution process
- Time saved in match management
- Reduced coaching stress during games

---

**Project Timeline**: 8-9 weeks for full implementation
**Target Users**: Youth soccer coaches, team managers, parents
**Primary Platform**: Web-based (Blazor WebAssembly) for cross-platform compatibility