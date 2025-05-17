using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;


namespace Server.Mobiles
{
    [CorpseName("a plague imp corpse")]
    public class PlagueImp : BaseCreature
    {
        // Timer control for the Plague Burst ability.
        private DateTime _NextPlagueBurst;

        [Constructable]
        public PlagueImp()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a Plague Imp";
            this.Body = 74;
            this.BaseSoundID = 422;

            // Unique hue to differentiate this advanced monster.
            this.Hue = 0x835;

            // Enhanced attributes for a more dangerous creature.
            this.SetStr(120, 150);
            this.SetDex(100, 120);
            this.SetInt(120, 140);

            this.SetHits(150, 200);
            this.SetDamage(15, 20);

            // Damage is split among physical, fire, and poison (with poison being significant).
            this.SetDamageType(ResistanceType.Physical, 30);
            this.SetDamageType(ResistanceType.Fire, 20);
            this.SetDamageType(ResistanceType.Poison, 50);

            // Improved resistances.
            this.SetResistance(ResistanceType.Physical, 35, 45);
            this.SetResistance(ResistanceType.Fire, 50, 60);
            this.SetResistance(ResistanceType.Cold, 25, 35);
            this.SetResistance(ResistanceType.Poison, 60, 70);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            // Elevated skills in magic and combat.
            this.SetSkill(SkillName.EvalInt, 40.0, 55.0);
            this.SetSkill(SkillName.Magery, 100.0, 120.0);
            this.SetSkill(SkillName.MagicResist, 80.0, 95.0);
            this.SetSkill(SkillName.Tactics, 70.0, 85.0);
            this.SetSkill(SkillName.Wrestling, 80.0, 95.0);
            this.SetSkill(SkillName.Necromancy, 50.0, 70.0);
            this.SetSkill(SkillName.SpiritSpeak, 50.0, 70.0);

            this.Fame = 5000;
            this.Karma = -5000;

            this.VirtualArmor = 40;

            // This advanced creature is not tamable.
            this.Tamable = false;
            this.ControlSlots = 0;
            this.MinTameSkill = 0;

            // Optionally, pack a rare plague-related loot item.
            if (Utility.RandomDouble() < 0.05)
                PackItem(new PlagueTalisman());
        }

        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != Map || 
                !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
            {
                return;
            }

            // Use the Plague Burst ability if the timer is up (roughly every 10 seconds)
            if (DateTime.UtcNow >= _NextPlagueBurst)
            {
                // 30% chance to trigger the burst on this action tick.
                if (Utility.RandomDouble() < 0.30)
                {
                    PlagueBurst(combatant);
                    _NextPlagueBurst = DateTime.UtcNow + TimeSpan.FromSeconds(10.0);
                }
            }
        }

        /// <summary>
        /// Launches a ranged toxic attack that deals poison damage and applies poison effects
        /// to the primary target and any nearby foes.
        /// </summary>
        public void PlagueBurst(Mobile target)
        {
            if (target == null || target.Deleted)
                return;

            // Ensure that Combatant is a Mobile before doing mobile-specific operations.
            if (Combatant is Mobile)
            {
                // Announce the ability activation overhead.
                PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "Plague Burst!");
            }

            // Visual effect: show moving particles (using a poison-like hue).

            // Delay the burst effect to simulate travel time of the toxic cloud.
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                if (target == null || target.Deleted)
                    return;
                if (!InRange(target, 12))
                    return;

                // Mark the attack as harmful.
                DoHarmful(target);

                // Calculate and apply the poison damage to the target.
                int baseDamage = Utility.RandomMinMax(20, 30);
                AOS.Damage(target, this, baseDamage, 0, 0, 0, 100, 0); // 100% poison damage

                // Apply a deadly poison if the target is not already poisoned.
                if (target is Mobile mTarget && !target.Poisoned)
                {
                    mTarget.ApplyPoison(this, Poison.Deadly);
                }

                // Additional AoE effect: targets within 2 tiles.
                List<Mobile> nearbyTargets = new List<Mobile>();
                foreach (Mobile m in target.GetMobilesInRange(2))
                {
                    if (m != target && m != this && CanBeHarmful(m))
                        nearbyTargets.Add(m);
                }
                foreach (Mobile m in nearbyTargets)
                {
                    DoHarmful(m);
                    int aoeDamage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, aoeDamage, 0, 0, 0, 100, 0);
                    if (!m.Poisoned)
                    {
                        m.ApplyPoison(this, Poison.Regular);
                    }
                }
            });
        }

        /// <summary>
        /// On a successful melee attack, there is a chance to apply a toxic "Plague Touch"
        /// which deals extra damage over time and applies a greater poison.
        /// </summary>
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender == null || defender.Deleted)
                return;

            // 20% chance to trigger the Plague Touch on melee attack.
            if (Utility.RandomDouble() < 0.20)
            {
                DoHarmful(defender);
                defender.SendMessage("You feel a virulent touch seeping into your veins!");
                // Use the defensive check: if (defender is Mobile target) before using Mobile properties.
                if (defender is Mobile target)
                {
                    target.ApplyPoison(this, Poison.Greater);
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, 2);
            // Rare plague-related item with a low chance.
            if (Utility.RandomDouble() < 0.01)
            {
                PackItem(new PlagueRelic());
            }
        }

        public override bool CanFly { get { return true; } }
        public override int Meat { get { return 1; } }
        public override int Hides { get { return 8; } } // Increased hides due to its advanced nature.
        public override HideType HideType { get { return HideType.Spined; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Daemon; } }

        public PlagueImp(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
