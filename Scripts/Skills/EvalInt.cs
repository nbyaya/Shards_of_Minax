using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.SkillHandlers
{
    public class EvalInt
    {
        // This dictionary maps specific node bit flags (from the EvalInt tree) to a bonus error margin reduction.
        // The bit flag values here are taken from the order in the EvalIntSkillTree:
        //   Keen Perception (0x02), Focused Intellect (0x40), Logical Rigor (0x400), Mental Fortitude (0x8000),
        //   Prime Intellect (0x20000), Expanded Cognition (0x200000), Cerebral Endowment (0x4000000), Ultimate Sage (0x20000000)
        public static readonly Dictionary<int, int> MarginReductionBonuses = new Dictionary<int, int>
        {
            { 0x02, 1 },
            { 0x40, 1 },
            { 0x400, 1 },
            { 0x8000, 1 },
            { 0x20000, 1 },
            { 0x200000, 1 },
            { 0x4000000, 1 },
            { 0x20000000, 1 }
        };

        public static void Initialize()
        {
            SkillInfo.Table[16].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.Target = new InternalTarget();
            m.SendLocalizedMessage(500906); // What do you wish to evaluate?
            return TimeSpan.FromSeconds(1.0);
        }

        private class InternalTarget : Target
        {
            public InternalTarget() : base(8, false, TargetFlags.None)
            {
            }

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (from == targeted)
				{
					from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 500910);
				}
				else if (targeted is TownCrier)
				{
					((TownCrier)targeted).PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500907, from.NetState);
				}
				else if (targeted is BaseVendor && ((BaseVendor)targeted).IsInvulnerable)
				{
					((BaseVendor)targeted).PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500909, from.NetState);
				}
				else if (targeted is Mobile targ)
				{
					int baseError = Math.Max(0, 20 - (int)(from.Skills[SkillName.EvalInt].Value / 5));
					int bonusReduction = 0;

					TalentProfile profile = null; // Define profile outside the if block

					if (from is PlayerMobile player) // Ensure 'from' is a PlayerMobile
					{
						profile = player.AcquireTalents();
						if (profile.Talents.ContainsKey(TalentID.EvalIntNodes))
						{
							int activatedNodes = profile.Talents[TalentID.EvalIntNodes].Points;
							foreach (var kvp in MarginReductionBonuses)
							{
								if ((activatedNodes & kvp.Key) != 0)
									bonusReduction += kvp.Value;
							}
						}
					}

					int marginOfError = Math.Max(0, baseError - bonusReduction);
					int intel = targ.Int + Utility.RandomMinMax(-marginOfError, +marginOfError);
					int mana = ((targ.Mana * 100) / Math.Max(targ.ManaMax, 1)) + Utility.RandomMinMax(-marginOfError, +marginOfError);

					int intMod = intel / 10;
					int mnMod = mana / 10;
					if (intMod > 10)
						intMod = 10;
					else if (intMod < 0)
						intMod = 0;
					if (mnMod > 10)
						mnMod = 10;
					else if (mnMod < 0)
						mnMod = 0;

					int body = targ.Body.IsHuman ? (targ.Female ? 11 : 0) : 22;
					if (from.CheckTargetSkill(SkillName.EvalInt, targ, 0.0, 120.0))
					{
						targ.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 1038169 + intMod + body, from.NetState);
						if (from.Skills[SkillName.EvalInt].Base >= 76.0)
							targ.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 1038202 + mnMod, from.NetState);
					}
					else
					{
						targ.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 1038166 + (body / 11), from.NetState);
					}
				}
				else if (targeted is Item)
				{
					((Item)targeted).SendLocalizedMessageTo(from, 500908, "");
				}
			}

        }
    }
}
