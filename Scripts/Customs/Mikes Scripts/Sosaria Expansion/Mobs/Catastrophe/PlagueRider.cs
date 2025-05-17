using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a diseased corpse")]
    public class PlagueRider : BaseCreature
    {
        private DateTime _nextPlagueCloud, _nextPlagueThrow;

        [Constructable]
        public PlagueRider()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a Plague Rider";
            this.Body = 185; // same body as Savage Rider
            this.BaseSoundID = 0x3E3; // example sound, adjust as desired

            // Increased stats
            this.SetStr(200, 220);
            this.SetDex(100, 140);
            this.SetInt(80, 100);
            this.SetHits(500, 600);

            this.SetDamage(35, 45);
            // Mix physical and poison damage: 70% physical, 30% poison
            this.SetDamageType(ResistanceType.Physical, 70);
            this.SetDamageType(ResistanceType.Poison, 30);

            // Resistances: notably high poison resistance
            this.SetResistance(ResistanceType.Physical, 50, 60);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 30, 40);
            this.SetResistance(ResistanceType.Poison, 60, 80);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            // Skills: sharpened melee, poisoning, and magical defenses
            this.SetSkill(SkillName.Fencing, 80.0, 100.0);
            this.SetSkill(SkillName.Macing, 80.0, 100.0);
            this.SetSkill(SkillName.Healing, 70.0, 90.0);
            this.SetSkill(SkillName.Poisoning, 80.0, 100.0);
            this.SetSkill(SkillName.MagicResist, 80.0, 95.0);
            this.SetSkill(SkillName.Swords, 80.0, 100.0);
            this.SetSkill(SkillName.Tactics, 80.0, 100.0);

            this.Fame = 7000;
            this.Karma = -7000;
            this.VirtualArmor = 40;

            // Unique hue for visual distinction (a dark, plague-ridden tone)
            this.Hue = 0x455;

            // Pack some basic items and a chance for plague-themed loot.
            this.PackItem(new Bandage(Utility.RandomMinMax(1, 20)));
            if (Utility.RandomDouble() < 0.2)
                this.PackItem(new PlagueVial()); // custom item? Replace or create as needed.

            // Equip with plague-themed weaponry and armor.
            this.AddItem(new PlagueSpear());   // custom, plague-themed weapon
            this.AddItem(new PlagueArmor());    // custom, thematic armor piece

            // Initialize ability timers to now.
            _nextPlagueCloud = DateTime.UtcNow;
            _nextPlagueThrow = DateTime.UtcNow;
        }

        public PlagueRider(Serial serial)
            : base(serial)
        {
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }

        // Generate loot as desired.
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Potions);
        }

        // Override combat action to use special abilities at intervals.
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            if (combatant == null || combatant.Deleted || combatant.Map != this.Map)
                return;

            // If in range (and line-of-sight) emit a Plague Cloud every 15 seconds.
            if (DateTime.UtcNow >= _nextPlagueCloud && InRange(combatant, 8) && CanBeHarmful(combatant) && InLOS(combatant))
            {
                ActivatePlagueCloud(combatant);
                _nextPlagueCloud = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            }

            // If target within 12 tiles, attempt to throw a Plague Vial every 10 seconds.
            if (DateTime.UtcNow >= _nextPlagueThrow && InRange(combatant, 12) && CanBeHarmful(combatant) && InLOS(combatant))
            {
                ThrowPlagueVial(combatant);
                _nextPlagueThrow = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            }
        }

        // Plague Cloud: emits a poisonous burst in an area around the target.
        private void ActivatePlagueCloud(Mobile target)
        {
            // Animate and play plague cloud sound effect.
            this.Animate(12, 5, 1, true, false, 0);
            this.PlaySound(0x22D); // sound effect for plague activation

            // Visual particle effects at our location.
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(1)), 0x3709, 10, 30, 0x455, 0, 5029, 0);

            // Affect all mobiles in a 3-tile radius.
            List<Mobile> nearby = new List<Mobile>();
            foreach (Mobile m in target.GetMobilesInRange(3))
            {
                if (m != null && m != this && CanBeHarmful(m))
                    nearby.Add(m);
            }
            foreach (Mobile m in nearby)
            {
                DoHarmful(m);
                // Always verify m is a Mobile before accessing properties.
                if (m is Mobile mobTarget)
                {
                    // Apply a potent poison (lethal) and reduce stamina.
                    mobTarget.ApplyPoison(mobTarget, Poison.Lethal);
                    mobTarget.Stam -= 20;
                    mobTarget.SendMessage("You feel your strength wither as a plague cloud engulfs you!");
                }
            }
        }

        // Plague Vial: the Plague Rider throws a vial that shatters upon impact, infecting the target.
        private void ThrowPlagueVial(Mobile target)
        {
            if (!(target is Mobile targetMobile))
                return;

            double distance = GetDistanceToSqrt(targetMobile);
            if (distance < 4 || distance > 15)
                return;

            // Trigger a throwing animation.
            this.Animate(10, 6, 1, true, false, 0);
            DoHarmful(targetMobile);

            // Moving particles for visual flair.
            this.MovingParticles(targetMobile, Utility.RandomList(0x36DA, 0x36DB, 0x36DC), 10, 5, false, true, 0, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0);

            // Delay to simulate the travel time of the vial.
            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                // Calculate a base 70% chance to hit.
                double hitChance = 0.7;
                if (Utility.RandomDouble() <= hitChance)
                {
                    targetMobile.PlaySound(0x0E6); // impact sound effect

                    // Calculate damage and deal pure poison damage.
                    int damage = Utility.RandomMinMax(20, 35);
                    AOS.Damage(targetMobile, this, damage, 0, 0, 0, 100, 0);

                    // Apply a deadly poison to the target.
                    targetMobile.ApplyPoison(targetMobile, Poison.Deadly);
                    targetMobile.SendMessage("The shattered vial infects you with a deadly plague!");

                    // Also affect nearby enemies with a reduced damage AoE effect.
                    foreach (Mobile m in targetMobile.GetMobilesInRange(2))
                    {
                        if (m != targetMobile && m != this && CanBeHarmful(m))
                        {
                            DoHarmful(m);
                            int aoeDamage = Utility.RandomMinMax(10, 20);
                            AOS.Damage(m, this, aoeDamage, 0, 0, 0, 100, 0);
                            m.ApplyPoison(m, Poison.Lesser);
                        }
                    }
                }
                else
                {
                    targetMobile.SendMessage("The Plague Vial misses you, shattering harmlessly on the ground!");
                    targetMobile.PlaySound(0x238);
                }
            });
        }

        // On a successful melee attack, add a chance to infect the defender with the plague.
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.3)
            {
                if (defender is Mobile target)
                {
                    target.ApplyPoison(target, Poison.Deadly);
                    target.SendMessage("The Plague Rider's blow infects you with a deadly disease!");
                }
            }
        }

        public override void AlterMeleeDamageTo(Mobile to, ref int damage)
        {
            // Increase damage against particularly resilient foes.
            if (to is Dragon || to is WhiteWyrm || to is SwampDragon)
                damage = (int)(damage * 1.5);
        }

		public override bool OnBeforeDeath()
		{
			// Release or delete any mount as with the original monster.
			IMount mount = this.Mount;
			if (mount != null)
			{
				mount.Rider = null;
				if (mount is Mobile)
					((Mobile)mount).Delete();
			}
			return base.OnBeforeDeath();
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
