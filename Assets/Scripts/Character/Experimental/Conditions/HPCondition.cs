using UnityEngine;
using System.Collections;

public class HPCondition : AIBaseCondition {
	public int hpThreshold;

	// Use this for initialization
	public override void Start () {
		base.Start();
	}

	public override bool check_condition ()
	{
        

		int hpPercentage = (int)(1.0f * (float)character.return_cur_stats().hp / 
		                         (float)character.return_base_stats().hp * 100.0f);
		if (hpPercentage <= hpThreshold && character.check_line_of_sight() == character.target)
			return true;

		else return false;
	}
}
