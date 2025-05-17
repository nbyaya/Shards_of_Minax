using System;
using Server;
using Server.Items;
using System.Collections;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a blighted shadow corpse")]
    public class BlightedShadow : BaseCreature
    {
        private DateTime _nextShadowBurst;
        private DateTime _nextDreadAura;

        [Constructable]
        public BlightedShadow()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a Blighted Shadow";
            this.Body = 740; // Same body as the Shadow Dweller
            this.Hue = 1175; // A unique hue to suggest its corrupted nature
            this.BaseSoundID = 0x5F1;


            // Enhanced Attributes
            this.SetStr(250, 300);
            this.SetDex(150, 170);
            this.SetInt(350, 400);

            this.SetHits(250, 300);
            this.SetDamage(30, 35);

            // Damage Types: mixing physical with cold and energy damage for a unique blend.
            this.SetDamageType(ResistanceType.Physical, 20);
            this.SetDamageType(ResistanceType.Cold, 30);
            this.SetDamageType(ResistanceType.Energy, 50);

            // Resistances: upgraded to reflect its advanced nature.
            this.SetResistance(ResistanceType.Physical, 50, 70);
            this.SetResistance(ResistanceType.Fire, 40, 50);
            this.SetResistance(ResistanceType.Cold, 60, 70);
            this.SetResistance(ResistanceType.Poison, 70, 80);
            this.SetResistance(ResistanceType.Energy, 50, 60);

            // Skills are also boosted.
            this.SetSkill(SkillName.EvalInt, 120.0, 130.0);
            this.SetSkill(SkillName.Magery, 90.1, 100.0);
            this.SetSkill(SkillName.Meditation, 95.1, 105.0);
            this.SetSkill(SkillName.MagicResist, 100.1, 120.0);
            this.SetSkill(SkillName.Tactics, 80.1, 90.0);

            this.Fame = 12000;
            this.Karma = -12000;
            this.VirtualArmor = 60;

            // Pack a higher range of reagents.
            this.PackNecroReg(25, 35);

            // Inherit LifeLeech from the base (for additional flavor)
            SetSpecialAbility(SpecialAbility.LifeLeech);
        }

        public BlightedShadow(Serial serial)
            : base(serial)
        {
        }

        // Advanced special abilities are triggered during combat.
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;
            if (DateTime.UtcNow < _nextShadowBurst || combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            // 50% chance to unleash a Shadow Burst on the current target.
            if (Utility.RandomDouble() < 0.50)
            {
                DoShadowBurst(combatant);
                _nextShadowBurst = DateTime.UtcNow + TimeSpan.FromSeconds(8.0 + (Utility.RandomDouble() * 4.0)); // delay between 8-12 seconds
            }

            // Also, every 15 seconds, trigger the Dread Aura that drains nearby foes.
            if (DateTime.UtcNow >= _nextDreadAura)
            {
                DoDreadAura();
                _nextDreadAura = DateTime.UtcNow + TimeSpan.FromSeconds(15.0);
            }
        }

        // Shadow Burst: launches a burst of dark energy toward the target.
        // It deals damage and leeches life if it hits.
        private void DoShadowBurst(Mobile target)
        {
            if (!(target is Mobile))
                return;

            DoHarmful(target);

            // Visual effect: moving particles from head towards the target.

            // Simulate travel delay
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), delegate
            {
                double hitChance = 0.65; // base 65% chance to hit
                if (Utility.RandomDouble() <= hitChance)
                {
                    int baseDamage = Utility.RandomMinMax(35, 45);
                    // Damage mix: energy and cold (50% each)
                    AOS.Damage(target, this, baseDamage, 0, 0, 50, 0, 50);

                    // Life leech effect: heal self for 50% of damage dealt
                    int healAmount = baseDamage / 2;
                    this.Hits += healAmount;
                    this.SendMessage("The Blighted Shadow siphons life from its foe!");
                }
                else
                {
                    target.SendAsciiMessage("The burst of shadow passes you by!");
                    target.PlaySound(0x1E);
                }
            });
        }

        // Dread Aura: emits a dark aura in a 2-tile radius that drains stamina and mana from enemies.
        private void DoDreadAura()
        {
            IPooledEnumerable inRange = this.GetMobilesInRange(2);
            foreach (Mobile m in inRange)
            {
                if (m != this && m is Mobile && CanBeHarmful(m))
                {
                    m.SendAsciiMessage("You feel your strength draining away...");
                    // When accessing Mobile-specific properties, check with "if (m is Mobile target)"
                    if (m is Mobile target)
                    {
                        target.Stam = Math.Max(0, target.Stam - 10);
                        target.Mana = Math.Max(0, target.Mana - 10);
                    }
                }
            }
            inRange.Free();
        }

        // When striking in melee, there is a chance to mark the target with a dark curse.
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.30)
            {
                defender.SendAsciiMessage("A dark mark burns into your skin!");
                // After a short delay, deal extra damage as the curse takes effect.
                Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
                {
                    if (defender != null && !defender.Deleted)
                    {
                        AOS.Damage(defender, this, Utility.RandomMinMax(10, 15), 0, 0, 0, 100, 0);
                    }
                });
            }
        }

        // Standard overrides
        public override bool CanRummageCorpses { get { return true; } }
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override int TreasureMapLevel { get { return 4; } }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich, 4);
            this.AddLoot(LootPack.MedScrolls, 3);
            
            // Rare chance to drop a unique artifact.
            if (Utility.RandomDouble() < 0.002)
            {
                this.PackItem(new ShadowweaversRobes());
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(_nextShadowBurst);
            writer.Write(_nextDreadAura);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _nextShadowBurst = reader.ReadDateTime();
            _nextDreadAura = reader.ReadDateTime();
        }
    }
}
