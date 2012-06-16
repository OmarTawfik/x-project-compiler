#pragma once

class UpdateArguments
{
private:
	float interframeTime;
	float timeSinceStart;
	int frameNumber;

public:
	UpdateArguments(float interframeTime, float timeSinceStart, int frameNumber)
	{
		this->interframeTime = interframeTime;
		this->timeSinceStart = timeSinceStart;
		this->frameNumber = frameNumber;
	}

	float GetInterframeTime()
	{
		return this->interframeTime;
	}

	float GetTimeSinceStart()
	{
		return this->timeSinceStart;
	}

	int GetFrameNumber()
	{
		return frameNumber;
	}
};
