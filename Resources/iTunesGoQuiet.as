# To make iTunes go quiet for 4 seconds:
# $ osacript iTunesGoQuiet.as 4

on run argv
	tell application "System Events" to set itunes_running to (name of processes) contains "iTunes"
	if itunes_running
		set seconds_quiet to item 1 of argv
		my itunes_go_quiet(seconds_quiet)
	end if
end run

on itunes_go_quiet(how_long)
	tell application "iTunes"
		set original_volume to sound volume
		my itunes_fade_volume(original_volume / 3)
		delay how_long
		my itunes_fade_volume(original_volume)
	end tell
end itunes_go_quiet

on itunes_fade_volume(target_volume)
	tell application "iTunes"
		set volume_change_increment to 3
		set volume_change_time to 0.5
		set volume_delta to target_volume - sound volume
		if volume_delta < 0 then set volume_change_increment to -volume_change_increment
		set volume_change_total_steps to volume_delta / volume_change_increment
		repeat volume_change_total_steps times
			set the sound volume to sound volume + volume_change_increment
			delay volume_change_time / volume_change_total_steps
		end repeat
		set the sound volume to target_volume
	end tell
end itunes_fade_volume