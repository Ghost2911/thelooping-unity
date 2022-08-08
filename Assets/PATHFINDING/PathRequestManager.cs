using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class PathRequestManager : MonoBehaviour
{
	Dictionary<int, PathRequest> pathRequestQueue = new Dictionary<int, PathRequest>();
	PathRequest currentPathRequest;

	static PathRequestManager instance;
	Pathfinding pathfinding;

	bool isProcessingPath;

	void Awake()
	{
		instance = this;
		pathfinding = GetComponent<Pathfinding>();
	}

	public static void RequestPath(int unitID, Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
	{
		PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
		if (instance.pathRequestQueue.ContainsKey(unitID))
			instance.pathRequestQueue[unitID] = newRequest;
		else
		{
			instance.pathRequestQueue.Add(unitID, newRequest);
			instance.TryProcessNext();
		}
	}

	public static void RemovePath(int unitID)
	{
		instance.pathRequestQueue.Remove(unitID);
	}

	void TryProcessNext()
	{
		if (!isProcessingPath && pathRequestQueue.Count > 0)
		{
			var curUnit = pathRequestQueue.First();
			currentPathRequest = curUnit.Value;
			instance.pathRequestQueue.Remove(curUnit.Key);
			isProcessingPath = true;
			pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
		}
	}

	public void FinishedProcessingPath(Vector3[] path, bool success)
	{
		if (currentPathRequest.callback!=null)
			currentPathRequest.callback(path, success);
		isProcessingPath = false;
		TryProcessNext();
	}

	struct PathRequest
	{
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Vector3[], bool> callback;

		public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
		{
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}
	}
}