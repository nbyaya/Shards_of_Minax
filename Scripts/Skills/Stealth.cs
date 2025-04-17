using System;
using Server.Items;
using Server.Mobiles;


namespace Server.SkillHandlers
{
    public class Stealth
    {
        private static readonly int[,] m_ArmorTable = new int[,]
        {
            //	Gorget	Gloves	Helmet	Arms	Legs	Chest	Shield
            /* Cloth	*/ { 0, 0, 0, 0, 0, 0, 0 },
            /* Leather	*/ { 0, 0, 0, 0, 0, 0, 0 },
            /* Studded	*/ { 2, 2, 0, 4, 6, 10, 0 },
            /* Bone		*/ { 0, 5, 10, 10, 15, 25, 0 },
            /* Spined	*/ { 0, 0, 0, 0, 0, 0, 0 },
            /* Horned	*/ { 0, 0, 0, 0, 0, 0, 0 },
            /* Barbed	*/ { 0, 0, 0, 0, 0, 0, 0 },
            /* Ring		*/ { 0, 5, 0, 10, 15, 25, 0 },
            /* Chain	*/ { 0, 0, 10, 0, 15, 25, 0 },
            /* Plate	*/ { 5, 5, 10, 10, 15, 25, 0 },
            /* Dragon	*/ { 0, 5, 10, 10, 15, 25, 0 }
        };

        public static double HidingRequirement => Core.ML ? 30.0 : (Core.SE ? 50.0 : 80.0);

        public static int[,] ArmorTable => m_ArmorTable;

        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Stealth].Callback = new SkillUseCallback(OnUse);
        }

        public static int GetArmorRating(Mobile m)
        {
            if (!Core.AOS)
                return (int)m.ArmorRating;

            int ar = 0;
            foreach (Item item in m.Items)
            {
                if (item is BaseArmor armor && armor.ArmorAttributes.MageArmor == 0)
                {
                    int materialType = (int)armor.MaterialType;
                    int bodyPosition = (int)armor.BodyPosition;

                    if (materialType < m_ArmorTable.GetLength(0) && bodyPosition < m_ArmorTable.GetLength(1))
                        ar += m_ArmorTable[materialType, bodyPosition];
                }
            }
            return ar;
        }

        public static TimeSpan OnUse(Mobile m)
        {
            if (!m.Hidden)
            {
                m.SendLocalizedMessage(502725); // You must hide first
            }
            else if (m.Flying)
            {
                m.SendLocalizedMessage(1113415); // You cannot use this ability while flying.
                m.RevealingAction();
                BuffInfo.RemoveBuff(m, BuffIcon.HidingAndOrStealth);
            }
            else if (m.Skills[SkillName.Hiding].Base < HidingRequirement)
            {
                m.SendLocalizedMessage(502726); // You are not hidden well enough. Become better at hiding.
                m.RevealingAction();
                BuffInfo.RemoveBuff(m, BuffIcon.HidingAndOrStealth);
            }
            else if (!m.CanBeginAction(typeof(Stealth)))
            {
                m.SendLocalizedMessage(1063086); // You cannot use this skill right now.
                m.RevealingAction();
                BuffInfo.RemoveBuff(m, BuffIcon.HidingAndOrStealth);
            }
            else
            {
                int armorRating = GetArmorRating(m);

                if (armorRating >= (Core.AOS ? 42 : 26))
                {
                    m.SendLocalizedMessage(502727); // You could not hope to move quietly wearing this much armor.
                    m.RevealingAction();
                    BuffInfo.RemoveBuff(m, BuffIcon.HidingAndOrStealth);
                }
                else if (m.CheckSkill(SkillName.Stealth, -20.0 + (armorRating * 2), (Core.AOS ? 60.0 : 80.0) + (armorRating * 2)))
                {
                    int baseSteps = (int)(m.Skills[SkillName.Stealth].Value / (Core.AOS ? 5.0 : 10.0));
                    if (baseSteps < 1)
                        baseSteps = 1;

                    // Integrate Stealth Tree Bonuses
                    if (m is PlayerMobile player)
                    {
                        var profile = player.AcquireTalents();

                        int bonusSteps = profile.Talents[TalentID.StealthStepsBonus].Points;
                        int speedBonus = profile.Talents[TalentID.StealthSpeedBonus].Points;
                        int dodgeBonus = profile.Talents[TalentID.StealthDodgeBonus].Points;
                        int detectionBonus = profile.Talents[TalentID.StealthDetectionBonus].Points;
                        int defenseBonus = profile.Talents[TalentID.StealthDefenseBonus].Points;
                        int recoveryBonus = profile.Talents[TalentID.StealthRecoveryBonus].Points;

                        // Apply stealth step bonus
                        m.AllowedStealthSteps = baseSteps + bonusSteps;

                        // Apply movement speed bonus
                        if (speedBonus > 0)
                        {
                            m.SendMessage($"Your silent movement is enhanced. Speed boost: +{speedBonus * 2}%.");
                        }

                        // Apply dodge chance bonus
                        if (dodgeBonus > 0)
                        {
                            m.SendMessage($"Your agility in the shadows improves. Dodge bonus: +{dodgeBonus * 3}%.");
                        }

                        // Apply detection resistance bonus
                        if (detectionBonus > 0)
                        {
                            m.SendMessage($"Your presence becomes harder to detect. Detection resistance: +{detectionBonus * 5}%.");
                        }

                        // Apply defensive bonus
                        if (defenseBonus > 0)
                        {
                            m.VirtualArmorMod += defenseBonus * 2;
                            m.SendMessage($"The shadows shield you. Defense bonus: +{defenseBonus * 2} armor.");
                        }

                        // Apply recovery bonus
                        if (recoveryBonus > 0)
                        {
                            m.SendMessage($"You recover from detection more quickly. Recovery time reduced by {recoveryBonus} seconds.");
                        }
                    }

                    m.IsStealthing = true;
                    m.SendLocalizedMessage(502730); // You begin to move quietly.

                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.HidingAndOrStealth, 1044107, 1075655));
                    return TimeSpan.FromSeconds(10.0);
                }
                else
                {
                    m.SendLocalizedMessage(502731); // You fail in your attempt to move unnoticed.
                    m.RevealingAction();
                    BuffInfo.RemoveBuff(m, BuffIcon.HidingAndOrStealth);
                }
            }

            return TimeSpan.FromSeconds(10.0);
        }
    }
}
