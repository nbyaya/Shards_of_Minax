using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a putrid tentacle corpse")]
    public class PutridTentacle : BaseCreature
    {
        private DateTime _NextSporeBurst;
        private int _SporeCount;

        [Constructable]
        public PutridTentacle()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a putrid tentacle";
            this.Body = 66;
            this.BaseSoundID = 352;
            this.Hue = 0x835; // Unique hue for the Putrid Tentacle

            // Stronger stats than the basic swamp tentacle
            this.SetStr(150, 180);
            this.SetDex(80, 100);
            this.SetInt(50, 70);

            this.SetHits(400, 450);
            this.SetMana(100, 120);

            this.SetDamage(15, 25);

            // Damage types: a mix of Physical and Poison
            this.SetDamageType(ResistanceType.Physical, 30);
            this.SetDamageType(ResistanceType.Poison, 70);

            // Resistances
            this.SetResistance(ResistanceType.Physical, 35, 45);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 80, 90);
            this.SetResistance(ResistanceType.Energy, 25, 35);

            // Skills
            this.SetSkill(SkillName.MagicResist, 60.0, 75.0);
            this.SetSkill(SkillName.Tactics, 80.0, 95.0);
            this.SetSkill(SkillName.Wrestling, 80.0, 95.0);

            this.Fame = 5000;
            this.Karma = -5000;

            this.VirtualArmor = 40;

            // Pack a little extra - could include reagents or poison items
            this.PackItem(new LesserPoisonPotion());
        }

        public PutridTentacle(Serial serial)
            : base(serial)
        {
        }

        // The Putrid Tentacle is immune to Deadly Poison
        public override Poison PoisonImmune
        {
            get { return Poison.Deadly; }
        }

        // Advanced loot including a chance at a rare toxin vial
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Potions);
        }

        // Unique ability: periodically release a burst of toxic spores
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            if (DateTime.UtcNow < _NextSporeBurst || combatant == null || combatant.Deleted || combatant.Map != Map ||
                !InRange(combatant, 10) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            ReleaseSpores(combatant);
            _SporeCount++;

            // Vary the delay between bursts: sometimes quick, sometimes longer
            if (Utility.RandomDouble() < 0.5 && (_SporeCount % 2) == 1)
                _NextSporeBurst = DateTime.UtcNow + TimeSpan.FromSeconds(4.0);
            else
                _NextSporeBurst = DateTime.UtcNow + TimeSpan.FromSeconds(8.0 + (5.0 * Utility.RandomDouble()));
        }

        // Method to release a poisonous spore burst affecting nearby foes
        public void ReleaseSpores(Mobile target)
        {
            // Show a visual particle effect for the spore burst
            this.MovingParticles(target, 0x36BD, 10, 5, false, true, 0, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                // Affect all mobiles within 3 tiles of the target
                foreach (Mobile m in target.GetMobilesInRange(3))
                {
                    if (m != this && CanBeHarmful(m) && InLOS(m))
                    {
                        DoHarmful(m);
                        int sporeDamage = Utility.RandomMinMax(10, 20);
                        AOS.Damage(m, this, sporeDamage, 0, 0, 0, 0, 100); // Entirely poison damage

                        // Check Mobile-specific properties safely
                        if (m is Mobile mobTarget)
                        {
                            // 50% chance to apply a Deadly poison effect
                            if (Utility.RandomDouble() < 0.5)
                                mobTarget.ApplyPoison(this, Poison.Deadly);
                        }
                    }
                }
            });
        }

        // Unique melee effect: a Tentacle Grab that saps energy and inflicts poison damage
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.3) // 30% chance to trigger the grab
            {
                if (defender is Mobile target)
                {
                    target.SendAsciiMessage("The Putrid Tentacle lashes out, ensnaring you in putrid tendrils!");
                    target.Freeze(TimeSpan.FromSeconds(3.0));

                    // Drain some mana if the target has any
                    if (target.Mana > 0)
                    {
                        int manaDrain = Utility.RandomMinMax(10, 20);
                        target.Mana = Math.Max(0, target.Mana - manaDrain);
                        target.SendAsciiMessage("You feel your magical energy being sapped!");
                    }

                    // Inflict a small burst of additional poison damage
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100);
                }
            }
        }

        // Decay Aura: a passive ability that occasionally inflicts minor decay damage on close foes
        public override void OnThink()
        {
            base.OnThink();

            // Every think cycle, there is a small chance to activate the decay aura
            if (Utility.RandomDouble() < 0.05)
            {
                foreach (Mobile m in this.GetMobilesInRange(2))
                {
                    if (m != this && CanBeHarmful(m))
                    {
                        DoHarmful(m);
                        int decayDamage = Utility.RandomMinMax(3, 7);
                        AOS.Damage(m, this, decayDamage, 0, 0, 0, 0, 100);

                        if (m is Mobile target)
                        {
                            target.SendAsciiMessage("The foul decay emanating from the Tentacle weakens you.");
                        }
                    }
                }
            }
        }

        // Allow the creature to rummage corpses
        public override bool CanRummageCorpses { get { return true; } }

        // Serialization
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
