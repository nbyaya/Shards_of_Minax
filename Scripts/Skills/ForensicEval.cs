using System;
using System.Linq;
using System.Text;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.SkillHandlers
{
    public interface IForensicTarget
    {
        void OnForensicEval(Mobile m);
    }

    public class ForensicEvaluation
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Forensics].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.Target = new ForensicTarget(m);
            m.RevealingAction();

            m.SendLocalizedMessage(501000); // Show me the crime.

            return TimeSpan.FromSeconds(1.0);
        }

        public class ForensicTarget : Target
        {
            private PlayerMobile _player;

            public ForensicTarget(Mobile from)
                : base(GetMaxRange(from), false, TargetFlags.None)
            {
                _player = from as PlayerMobile;
            }

            private static int GetMaxRange(Mobile from)
            {
                if (from is PlayerMobile player)
                {
                    var profile = player.AcquireTalents();
                    return 10 + profile.Talents[(TalentID)TalentID.ForensicInsight].Points; // Base 10 + bonus range
                }
                return 10;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (_player == null)
                    return;

                var profile = _player.AcquireTalents();
                int efficiencyBonus = profile.Talents[(TalentID)TalentID.ForensicEfficiency].Points;
                int revelationBonus = profile.Talents[(TalentID)TalentID.ForensicRevelation].Points;

                double minSkillCorpse = Math.Max(10.0, 30.0 - efficiencyBonus); // Reduce corpse analysis requirement
                double minSkillMobile = Math.Max(15.0, 36.0 - efficiencyBonus); // Reduce thief detection requirement
                double minSkillLocks = Math.Max(20.0, 41.0 - efficiencyBonus); // Reduce lock analysis requirement

                if (target is Corpse corpse)
                {
                    if (_player.Skills[SkillName.Forensics].Value < minSkillCorpse)
                    {
                        from.SendLocalizedMessage(501003); // You notice nothing unusual.
                        return;
                    }

                    if (_player.CheckTargetSkill(SkillName.Forensics, target, minSkillCorpse, 55.0))
                    {
                        if (corpse.m_Forensicist != null)
                            from.SendMessage($"This crime has already been examined by {_player.Name}.");
                        else
                            corpse.m_Forensicist = _player.Name;

                        if (((Body)corpse.Amount).IsHuman)
                        {
                            from.SendMessage($"This person was killed by {corpse.Killer?.Name ?? "an unknown assailant"}.");
                        }

                        if (corpse.Looters.Count > 0)
                        {
                            var looters = string.Join(", ", corpse.Looters.Select(l => ((Mobile)l).Name));
                            from.SendMessage($"The body has been disturbed by: {looters}");
                        }
                        else
                        {
                            from.SendLocalizedMessage(501002); // The corpse has not been desecrated.
                        }

                        if (revelationBonus > 0)
                        {
                            from.SendMessage($"Forensic Revelation reveals deeper clues... (Bonus: {revelationBonus})");
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(501001); // You cannot determine anything useful.
                    }
                }
                else if (target is Mobile mobileTarget)
                {
                    if (_player.Skills[SkillName.Forensics].Value < minSkillMobile)
                    {
                        from.SendLocalizedMessage(501003); // You notice nothing unusual.
                        return;
                    }

                    if (_player.CheckTargetSkill(SkillName.Forensics, target, minSkillMobile, 100.0))
                    {
                        if (mobileTarget is PlayerMobile playerMobile && playerMobile.NpcGuild == NpcGuild.ThievesGuild)
                        {
                            from.SendMessage($"Forensic insight reveals that {playerMobile.Name} is a **thief**!");
                        }
                        else
                        {
                            from.SendLocalizedMessage(501003); // You notice nothing unusual.
                        }

                        if (revelationBonus >= 5)
                        {
                            from.SendMessage($"Forensic Revelation uncovers additional behavior patterns...");
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(501001); // You cannot determine anything useful.
                    }
                }
                else if (target is ILockpickable lockpickable)
                {
                    if (_player.Skills[SkillName.Forensics].Value < minSkillLocks)
                    {
                        from.SendLocalizedMessage(501003); // You notice nothing unusual.
                        return;
                    }

                    if (_player.CheckTargetSkill(SkillName.Forensics, target, minSkillLocks, 100.0))
                    {
                        if (lockpickable.Picker != null)
                        {
                            from.SendMessage($"This lock was opened by {lockpickable.Picker.Name}.");
                        }
                        else
                        {
                            from.SendLocalizedMessage(501003); // You notice nothing unusual.
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(501001); // You cannot determine anything useful.
                    }
                }
                else if (target is IForensicTarget forensicTarget)
                {
                    forensicTarget.OnForensicEval(from);
                }
                else if (target is Item item)
                {
                    var honestySocket = item.GetSocket<HonestyItemSocket>();

                    if (honestySocket != null)
                    {
                        if (honestySocket.HonestyOwner == null)
                            Server.Services.Virtues.HonestyVirtue.AssignOwner(honestySocket);

                        if (_player.CheckTargetSkill(SkillName.Forensics, target, minSkillLocks, 100.0))
                        {
                            string region = honestySocket.HonestyRegion ?? "an unknown place";
                            if (_player.Skills.Forensics.Value >= 61.0)
                            {
                                from.SendMessage($"This item belongs to {honestySocket.HonestyOwner.Name}, who lives in {region}.");
                            }
                            else
                            {
                                from.SendMessage($"You find evidence suggesting this item comes from {region}.");
                            }
                        }
                    }
                }
            }
        }
    }
}
