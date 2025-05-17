using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a pestilent skeletal corpse")]
    public class PestilentBoneMagi : BaseCreature
    {
        private DateTime _NextShardVolley;
        private DateTime _NextCurse;

        [Constructable]
        public PestilentBoneMagi()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Base Appearance and Attributes (using similar Body and Sound as BoneMagi)
            this.Name = "a pestilent bone magi";
            this.Body = 148;
            this.BaseSoundID = 451;


            // Set a unique hue (a sickly, greenish tint)
            this.Hue = 0x83F; // Adjust value as desired for a unique look

            // Attributes (advanced stats)
            this.SetStr(100, 140);
            this.SetDex(80, 100);
            this.SetInt(220, 260);
            this.SetHits(120, 150);

            // Damage output is enhanced
            this.SetDamage(10, 20);
            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Poison, 50);

            // Resistances (improved, with emphasis on poison and cold)
            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 40, 50);
            this.SetResistance(ResistanceType.Cold, 60, 70);
            this.SetResistance(ResistanceType.Poison, 50, 60);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            // Skills (boosted necromantic and magical powers)
            this.SetSkill(SkillName.EvalInt, 70.1, 80.0);
            this.SetSkill(SkillName.Magery, 70.1, 80.0);
            this.SetSkill(SkillName.MagicResist, 65.1, 80.0);
            this.SetSkill(SkillName.Tactics, 55.1, 70.0);
            this.SetSkill(SkillName.Wrestling, 50.1, 65.0);
            this.SetSkill(SkillName.Necromancy, 99.1, 110.0);
            this.SetSkill(SkillName.SpiritSpeak, 95.0, 105.0);

            this.Fame = 6000;
            this.Karma = -6000;

            this.VirtualArmor = 45;

            // Loot: In addition to standard loot, chance for special items
            PackReg(5);
            PackNecroReg(5, 15);
            PackItem(new Bone());
            if (Utility.RandomDouble() < 0.005) // 0.5% chance for a rare item
            {
                this.PackItem(new PlagueVial());
            }

            // Initialize ability timers
            _NextShardVolley = DateTime.UtcNow + TimeSpan.FromSeconds(8.0);
            _NextCurse = DateTime.UtcNow + TimeSpan.FromSeconds(12.0);
        }

        public PestilentBoneMagi(Serial serial)
            : base(serial)
        {
        }

        // This creature is immune to bleeding and normal poison (but not its own pestilent curse)
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Regular; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }
        public override TribeType Tribe { get { return TribeType.Undead; } }

        // Override the combat action to periodically use our unique abilities:
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != this.Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            DateTime now = DateTime.UtcNow;

            // 50% chance to launch Bone Shard Volley if cooldown expired
            if (now >= _NextShardVolley && Utility.RandomDouble() < 0.5)
            {
                ThrowBoneShards(combatant);
                _NextShardVolley = now + TimeSpan.FromSeconds(10.0 + (10.0 * Utility.RandomDouble()));
            }
            // 30% chance to cast the Pestilent Curse if cooldown expired
            else if (now >= _NextCurse && Utility.RandomDouble() < 0.3)
            {
                CastPestilentCurse(combatant);
                _NextCurse = now + TimeSpan.FromSeconds(15.0 + (5.0 * Utility.RandomDouble()));
            }
        }

        // Ability 1: Bone Shard Volley
        public void ThrowBoneShards(Mobile m)
        {
            // Verify target is Mobile before proceeding
            if (m == null || m.Deleted)
                return;

            // Define range parameters for the shard volley
            int minRange = 3;
            int maxRange = 14;
            double distance = GetDistanceToSqrt(m);

            if (distance >= minRange && distance <= maxRange)
            {
                // Trigger a casting animation or effect if desired
                Animate(12, 5, 1, true, false, 0); // Animation ID 12 = spellcasting for humanoid bones
                DoHarmful(m);

                // Launch bone shards: add moving particles for visual effect
                MovingParticles(m, Utility.RandomList(0x3E90, 0x3E91, 0x3E92), 15, 0, false, true, 0, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0);

                // Delay call to simulate travel time of the shards (1 second)
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
                {
                    // Calculate hit chance based on magery and necromancy skills
                    double chance = (this.Skills[SkillName.Magery].Value + this.Skills[SkillName.Necromancy].Value) / 200.0;
                    chance = Math.Max(0.25, Math.Min(0.95, chance)); // Clamp between 25% and 95%

                    if (Utility.RandomDouble() <= chance)
                    {
                        // On hit, deal mixed physical and poison damage
                        m.PlaySound(0x308);
                        int rawDamage = Utility.RandomMinMax(12, 18);

                        // Apply damage reduction based on target resistances (assume 50% poison / 50% physical)
                        int physicalReduction = m.PhysicalResistance;
                        int poisonReduction = m.PoisonResistance;

                        int physicalDamage = (int)(rawDamage * 0.5 * (1.0 - (physicalReduction / 100.0)));
                        int poisonDamage = (int)(rawDamage * 0.5 * (1.0 - (poisonReduction / 100.0)));
                        int totalDamage = Math.Max(1, physicalDamage + poisonDamage);

                        AOS.Damage(m, this, totalDamage, 50, 0, 0, 50, 0);

                        // Optionally, apply a brief movement slow (simulate being pained by splintering bones)
                        m.Freeze(TimeSpan.FromSeconds(1.0));
                        m.SendAsciiMessage("Your limbs ache as splintering bones tear into you!");
                    }
                    else
                    {
                        // If the shard volley misses, notify the target
                        m.SendAsciiMessage("The pestilent bone shards whiz past you!");
                        m.PlaySound(0x238);
                    }
                });
            }
        }

        // Ability 2: Pestilent Curse - drains mana and inflicts poison damage
        public void CastPestilentCurse(Mobile m)
        {
            // Verify target is valid before proceeding
            if (m == null || m.Deleted)
                return;

            DoHarmful(m);

            // Send feedback to the target
            m.SendAsciiMessage("A putrid curse saps your strength and drains your energies!");

            // If target is Mobile, adjust mana carefully
            if (m is Mobile target)
            {
                // Drain a percentage of their current mana (say 20%)
                int manaDrain = (int)(target.Mana * 0.20);
                target.Mana = Math.Max(0, target.Mana - manaDrain);
            }

            // Inflict immediate poison/necrotic damage as part of the curse
            int curseDamage = Utility.RandomMinMax(15, 25);

            // Apply damage with a 100% poison component
            AOS.Damage(m, this, curseDamage, 0, 0, 0, 100, 0);

            // Optionally, you could add a delayed damage over time component with a Timer here.
        }

        // Standard loot generation with higher rewards
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Potions);
        }

        public override bool CanRummageCorpses { get { return true; } }

        public override int TreasureMapLevel { get { return 4; } }

        public override int Meat { get { return 1; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset ability timers on load
            _NextShardVolley = DateTime.UtcNow + TimeSpan.FromSeconds(8.0);
            _NextCurse = DateTime.UtcNow + TimeSpan.FromSeconds(12.0);
        }
    }
}
