# level completion time
import matplotlib.pyplot as plt
import json

# The path to your JSON file
json_file_path = ‘comp_time.json’

# Load JSON data from the file
with open(json_file_path, 'r') as file:
    completion_times_data = json.load(file)

# Extract levels from the keys of the dictionary
levels = list(completion_times_data.keys())

# Plotting the completion times
plt.figure(figsize=(15, 8))

# Plot each player's completion time for each level
for level, times in completion_times_data.items():
    plt.scatter([level for _ in range(len(times))], times, alpha=0.6)

# Calculate and plot the average completion time for each level
average_times = [np.mean(times) for times in completion_times_data.values()]
plt.plot(levels, average_times, color='red', linestyle='-', marker='o', label='Average Completion Time')

# Set plot titles and labels
plt.title('Completion Times Across Levels for 24 Players')
plt.xlabel('Level')
plt.ylabel('Completion Time (seconds)')
plt.legend()
plt.grid(True)
plt.show()

# Print the average completion times for each level
for level, avg_time in zip(levels, average_times):
    print(f"{level}: Average completion time is {avg_time:.2f} seconds.")


#color switch count
file_path = ‘hue_hustlers_colorswitch.json’
with open(file_path, 'r') as file:
    updated_switch_counts_data = json.load(file)

plot_data = []
for level, playthroughs in updated_switch_counts_data.items():
    for playthrough_id, switch_count in playthroughs.items():
        plot_data.append((level, playthrough_id, switch_count))

# Create a DataFrame from the list of tuples
df = pd.DataFrame(plot_data, columns=['Level', 'PlaythroughID', 'SwitchCount'])

# Create a new figure for the scatter plot
plt.figure(figsize=(14, 8))

# Group the data by level and plot
for level, group_df in df.groupby('Level'):
    plt.scatter([level] * len(group_df), group_df['SwitchCount'], alpha=0.6)

# Formatting the plot
plt.xlabel('Level')
plt.ylabel('Switch Count')
plt.title('Scatter Plot of Switch Counts for Each Level)
plt.xticks(rotation=45)  # Rotate the x-axis labels for better readability
plt.tight_layout()  # Adjust the plot to ensure everything fits without overlapping

# Show the plot
plt.show()



# Zone Time For Each level: 

# First, let's import the necessary library
import matplotlib.pyplot as plt
import json

# Now, let's load the JSON data from the file
json_file_path = '/mnt/data/updated_hue-hustlers-default-rtdb-export_tutorial_2_right_side.json'

# Load JSON data from the file
with open(json_file_path, 'r') as file:
    tutorial_zone_times = json.load(file)

# Extract zone times for selected zones in 'Tutorial-1' and 'Tutorial-2'
selected_zones = {
    'Tutorial-1': {
        'Bomb Shoot': tutorial_zone_times['zonetimes']['Tutorial-1']['Bomb Shoot'],
        'Bomb Instant Shoot': tutorial_zone_times['zonetimes']['Tutorial-1']['Bomb Instant Shoot']
    },
    'Tutorial-2': {
        'Ditch': tutorial_zone_times['zonetimes']['Tutorial-2']['Ditch'],
        'Left side': tutorial_zone_times['zonetimes']['Tutorial-2']['Left side'],
        'Right side': tutorial_zone_times['zonetimes']['Tutorial-2']['Right side']
    }
}

# Prepare data for plotting for the selected zones
selected_plot_data = []
selected_labels = []
for level, zones in selected_zones.items():
    for zone, times in zones.items():
        # Extract only the time values for each zone
        time_values = [zone_info['ZoneTime'] for zone_info in times.values()]
        selected_plot_data.append(time_values)
        selected_labels.append(f"{level} - {zone}")

# Plotting for the selected zones in 'Tutorial-1' and 'Tutorial-2'
plt.figure(figsize=(12, 7))
plt.boxplot(selected_plot_data, notch=True)
plt.xticks(range(1, len(selected_labels) + 1), selected_labels, rotation=45, ha="right")
plt.ylabel('Time (seconds)')
plt.xlabel('Zones')
plt.title('Time Spent in Selected Zones of Tutorials 1 and 2')
plt.tight_layout()  # Adjust layout to prevent clipping of tick-labels
plt.show()


#bombenemydetonatedstatus

import matplotlib.pyplot as plt
import json

with open(‘bomb_status.json', 'r') as file:
    data = json.load(file)

# Specify the levels you want to include
selected_levels = ['Level-1', 'Level-2', 'Level-3', 'Level-4', 'Tutorial-1', 'Tutorial-2']

# Extract the SwitchCount values for the selected levels for plotting
levels = []
switch_counts = []

for level in selected_levels:
    if level in data:
        level_data = data[level]
        for switch_id, switch_data in level_data["switchCounts"].items():
            levels.append(level)
            switch_counts.append(switch_data["SwitchCount"])

# Create a scatter plot
plt.scatter(levels, switch_counts)
plt.xlabel("Level")
plt.ylabel("SwitchCount Value")
plt.title("SwitchCount for Selected Levels")
plt.grid(True)

plt.xticks(rotation=45)  # Rotate x-axis labels for better visibility

plt.show()