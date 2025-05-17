using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("Somnira's essence")]
    public class Somnira : BaseCreature
    {
        private DateTime _NextDreamBurst;
        private DateTime _NextEnergyDrain;
        private DateTime _NextRealityTwist;

        [Constructable]
        public Somnira()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Somnira";
            Body = 740;
            Hue = 0x481;
            BaseSoundID = 0x482;

            SetStr(700, 800);
            SetDex(250, 350);
            SetInt(900, 1000);

            SetHits(2500, 3000);
            SetDamage(25, 35);

            // Damage types (rolled "chaos" into energy)
            SetDamageType(ResistanceType.Physical, 5);
            SetDamageType(ResistanceType.Cold,     40);
            SetDamageType(ResistanceType.Energy,   55);

            // Resistances (no chaos)
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     50, 60);
            SetResistance(ResistanceType.Cold,     50, 60);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   65, 75);

            // Core skills only
            SetSkill(SkillName.Necromancy,    110.0, 130.0);
            SetSkill(SkillName.SpiritSpeak,   110.0, 130.0);
            SetSkill(SkillName.EvalInt,       110.0, 130.0);
            SetSkill(SkillName.Magery,        110.0, 130.0);
            SetSkill(SkillName.Meditation,    110.0, 120.0);
            SetSkill(SkillName.MagicResist,   130.0, 160.0);
            SetSkill(SkillName.Tactics,        80.0,  90.0);
            SetSkill(SkillName.Wrestling,     100.0, 110.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 40;

            PackReg(20);
            PackItem(new ArcaneGem());
        }

        public Somnira(Serial serial)
            : base(serial)
        {
        }

        public override bool BleedImmune          => true;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;
        public override Poison PoisonImmune       => Poison.Lethal;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.HighScrolls);
            // Removed non-existent ArtifactRarityCollection
        }

        public override int GetIdleSound()  => 0x5F4;
        public override int GetAngerSound() => 0x5F1;
        public override int GetDeathSound() => 0x5F2;
        public override int GetHurtSound()  => 0x5F3;

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            var combatant = Combatant;
            if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 15) || !CanBeHarmful(combatant))
                return;

            if (DateTime.UtcNow >= _NextDreamBurst && InLOS(combatant) && InRange(combatant, 8))
            {
                PerformDreamBurst(combatant);
                _NextDreamBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }

            if (DateTime.UtcNow >= _NextEnergyDrain && InLOS(combatant) && InRange(combatant, 5) && combatant is Mobile drainTarget)
            {
                PerformEnergyDrain(drainTarget);
                _NextEnergyDrain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
            }

            if (DateTime.UtcNow >= _NextRealityTwist && InLOS(combatant) && InRange(combatant, 10) && combatant is Mobile twistTarget)
            {
                PerformRealityTwist(twistTarget);
                _NextRealityTwist = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
        }

        // --- Dream Burst: 2‑sec wind‑up, AOE psychic + stam drain + possible confusion
        public void PerformDreamBurst(IDamageable target)
        {
            Animate(AnimationType.Attack, 0);
            Say("Reality shatters!");

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
            {
                var map = Map;
                if (map == null) return;

                var loc = Location;
                Effects.PlaySound(loc, map, 0x225);

                foreach (var m in map.GetMobilesInRange(loc, 6))
                {
                    if (m != this && CanBeHarmful(m) && m is Mobile mob)
                    {
                        DoHarmful(mob);

                        // Psychic damage
                        int dmg = Utility.RandomMinMax(30, 50);
                        AOS.Damage(mob, this, dmg, 0, 0, 0, 0, 100);

                        // Stam drain
                        int sd = Utility.RandomMinMax(20, 40);
                        mob.Stam -= sd;
                        mob.SendLocalizedMessage(1060091);

                        // 30% confusion
                        if (Utility.RandomDouble() < 0.3)
                        {
                            mob.FixedParticles(0x3735, 1, 30, 9961, 252, 7, EffectLayer.Waist);
                            mob.SendAsciiMessage("Your senses are distorted!");
                        }
                    }
                }

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, map, EffectItem.DefaultDuration),
                    0x3728, 10, 10, 252, 7
                );
            });
        }

        // --- Energy Drain: single target mana + stam siphon
        public void PerformEnergyDrain(Mobile target)
        {
            Animate(AnimationType.Attack, 0);

            int manaDrain = Utility.RandomMinMax(40, 60);
            int stamDrain = Utility.RandomMinMax(30, 50);

            if (target.Mana >= manaDrain)
            {
                target.Mana -= manaDrain;
                SayTo(target, "Your will falters!");
            }
            else if (target.Mana > 0)
            {
                target.Mana = 0;
                SayTo(target, "Your will falters!");
            }

            target.FixedParticles(0x374A, 10, 15, 5028, 252, 7, EffectLayer.Head);

            if (target.Stam >= stamDrain)
            {
                target.Stam -= stamDrain;
                target.SendLocalizedMessage(1060091);
            }
            else if (target.Stam > 0)
            {
                target.Stam = 0;
                target.SendLocalizedMessage(1060091);
            }

            // Somnira recovers half
            Mana += manaDrain / 2;
            Stam += stamDrain / 2;

            DoHarmful(target);
        }

        // --- Reality Twist: debuff resistances for 8s
        public void PerformRealityTwist(Mobile target)
        {
            Animate(AnimationType.Attack, 0);
            Say("Reality bends!");
            target.FixedParticles(0x3709, 10, 30, 5052, 252, 7, EffectLayer.Head);

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                if (target == null || target.Deleted) return;

                var mods = new[]
                {
                    new ResistanceMod(ResistanceType.Physical, -15),
                    new ResistanceMod(ResistanceType.Fire,     -15),
                    new ResistanceMod(ResistanceType.Cold,     -15),
                    new ResistanceMod(ResistanceType.Poison,   -15),
                    new ResistanceMod(ResistanceType.Energy,   -15)
                };

                foreach (var mod in mods)
                    target.AddResistanceMod(mod);

                target.SendLocalizedMessage(1019039);

                Timer.DelayCall(TimeSpan.FromSeconds(8.0), () =>
                {
                    if (target == null || target.Deleted) return;
                    foreach (var mod in mods)
                        target.RemoveResistanceMod(mod);
                    target.SendLocalizedMessage(1019038);
                });
            });

            DoHarmful(target);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
