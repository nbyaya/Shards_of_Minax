using System;
using Server.Engines.CannedEvil;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    [FlipableAttribute(0xE81, 0xE82)]
    public class ShepherdsCrook : BaseStaff
    {
        [Constructable]
        public ShepherdsCrook()
            : base(0xE81)
        {
            Weight = 4.0;
            ApplyRandomTier(); // Apply tiered damage on creation
        }

        public ShepherdsCrook(Serial serial)
            : base(serial)
        {
        }

        private void ApplyRandomTier()
        {
            Random rand = new Random();
            double chanceForSpecialTier = rand.NextDouble();

            // 50% chance for default stats, 50% chance for a special tier
            if (chanceForSpecialTier < 0.5)
            {
                return; // Default stats
            }

            // Determine special tier
            double tierChance = rand.NextDouble();

            if (tierChance < 0.05)
            {
                this.MinDamage = rand.Next(1, 80);
                this.MaxDamage = rand.Next(80, 120);
            }
            else if (tierChance < 0.2)
            {
                this.MinDamage = rand.Next(1, 70);
                this.MaxDamage = rand.Next(70, 100);
            }
            else if (tierChance < 0.5)
            {
                this.MinDamage = rand.Next(1, 50);
                this.MaxDamage = rand.Next(50, 75);
            }
            else
            {
                this.MinDamage = rand.Next(1, 30);
                this.MaxDamage = rand.Next(30, 50);
            }
        }

        private double _herdingBonus = 0.0;

        public override bool OnEquip(Mobile from)
        {
            if (from is PlayerMobile pm)
            {
                double herdingSkill = pm.Skills[SkillName.Herding].Value;
                double bonus = herdingSkill * 0.10;

                var profile = pm.AcquireTalents();
                if (!profile.Talents.TryGetValue(TalentID.MinionDamageBonus, out Talent talent))
                {
                    talent = new Talent(TalentID.MinionDamageBonus);
                    profile.Talents[TalentID.MinionDamageBonus] = talent;
                }

                _herdingBonus = bonus;
                talent.Points += (int)(_herdingBonus / 0.05); // Assuming 5% = 1 point

                pm.SendMessage(0x3B2, $"The Shepherd's Crook grants your minions +{bonus * 5:0.0}% damage based on your Herding skill.");
            }

            return base.OnEquip(from);
        }

        public override void OnRemoved(object parent)
        {
            if (parent is PlayerMobile pm && _herdingBonus > 0)
            {
                var profile = pm.AcquireTalents();
                if (profile.Talents.TryGetValue(TalentID.MinionDamageBonus, out Talent talent))
                {
                    int pointsToRemove = (int)(_herdingBonus / 0.05);
                    talent.Points = Math.Max(0, talent.Points - pointsToRemove);
                }

                pm.SendMessage(0x3B2, $"The Shepherd's Crook's minion bonus has worn off.");
                _herdingBonus = 0;
            }

            base.OnRemoved(parent);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (RootParent is PlayerMobile pm)
            {
                double herdingSkill = pm.Skills[SkillName.Herding].Value;
                double bonusPoints = herdingSkill * 0.10;
                double totalBonusPercent = bonusPoints * 5;

                list.Add($"Minion Damage Bonus: +{totalBonusPercent:0.0}% (from Herding)");
            }
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.CrushingBlow;
        public override WeaponAbility SecondaryAbility => WeaponAbility.Disarm;

        public override int AosStrengthReq => 20;
        public override int AosMinDamage => 13;
        public override int AosMaxDamage => 16;
        public override int AosSpeed => 40;
        public override float MlSpeed => 2.75f;

        public override int OldStrengthReq => 10;
        public override int OldMinDamage => 3;
        public override int OldMaxDamage => 12;
        public override int OldSpeed => 30;

        public override int InitMinHits => 31;
        public override int InitMaxHits => 50;

        public override SkillName DefSkill => SkillName.Herding;

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: Herding");
        }

        public override bool CanBeWornByGargoyles => true;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendLocalizedMessage(502464); // Target the animal you wish to herd.
            from.Target = new HerdingTarget(this);
        }

        // HerdingTarget class remains unchanged...
        private class HerdingTarget : Target
        {
            private static readonly Type[] m_ChampTamables = new Type[]
            {
                typeof(StrongMongbat), typeof(Imp), typeof(Scorpion), typeof(GiantSpider),
                typeof(Snake), typeof(LavaLizard), typeof(Drake), typeof(Dragon),
                typeof(Kirin), typeof(Unicorn), typeof(GiantRat), typeof(Slime),
                typeof(DireWolf), typeof(HellHound), typeof(DeathwatchBeetle),
                typeof(LesserHiryu), typeof(Hiryu)
            };

            private ShepherdsCrook m_Crook;

            public HerdingTarget(ShepherdsCrook crook)
                : base(10, false, TargetFlags.None)
            {
                m_Crook = crook;
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                if (targ is BaseCreature)
                {
                    BaseCreature bc = (BaseCreature)targ;

                    if (IsHerdable(bc))
                    {
                        if (bc.Controlled)
                        {
                            bc.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 502467, from.NetState); // That animal looks tame already.
                        }
                        else 
                        {
                            from.SendLocalizedMessage(502475); // Click where you wish the animal to go.
                            from.Target = new InternalTarget(bc, m_Crook);
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(502468); // That is not a herdable animal.
                    }
                }
                else
                {
                    from.SendLocalizedMessage(502472); // You don't seem to be able to persuade that to move.
                }
            }

            private bool IsHerdable(BaseCreature bc)
            {
                if (bc.IsParagon)
                    return false;

                if (bc.Tamable)
                    return true;

                Map map = bc.Map;

                ChampionSpawnRegion region = Region.Find(bc.Home, map) as ChampionSpawnRegion;

                if (region != null)
                {
                    ChampionSpawn spawn = region.ChampionSpawn;

                    if (spawn != null && spawn.IsChampionSpawn(bc))
                    {
                        Type t = bc.GetType();

                        foreach (Type type in m_ChampTamables)
                            if (type == t)
                                return true;
                    }
                }

                return false;
            }

            private class InternalTarget : Target
            {
                private BaseCreature m_Creature;
                private ShepherdsCrook m_Crook;

                public InternalTarget(BaseCreature c, ShepherdsCrook crook)
                    : base(10, true, TargetFlags.None)
                {
                    m_Creature = c;
                    m_Crook = crook;
                }

                protected override void OnTarget(Mobile from, object targ)
                {
                    if (targ is IPoint2D)
                    {
                        double min = m_Creature.CurrentTameSkill - 30;
                        double max = m_Creature.CurrentTameSkill + 30 + Utility.Random(10);

                        if (max <= from.Skills[SkillName.Herding].Value)
                            m_Creature.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 502471, from.NetState); // That wasn't even challenging.

                        if (from.CheckTargetSkill(SkillName.Herding, m_Creature, min, max))
                        {
                            IPoint2D p = (IPoint2D)targ;

                            if (targ != from)
                                p = new Point2D(p.X, p.Y);

                            m_Creature.TargetLocation = p;
                            from.SendLocalizedMessage(502479); // The animal walks where it was instructed to.

                            if (Siege.SiegeShard && m_Crook is IUsesRemaining)
                            {
                                Siege.CheckUsesRemaining(from, m_Crook);
                            }
                        }
                        else
                        {
                            from.SendLocalizedMessage(502472); // You don't seem to be able to persuade that to move.
                        }
                    }
                }
            }
        }		
    }
}
