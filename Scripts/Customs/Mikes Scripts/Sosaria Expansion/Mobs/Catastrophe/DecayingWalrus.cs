using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Misc; // for Effects and Timer if needed

namespace Server.Mobiles
{
    [CorpseName("a decaying walrus corpse")]
    public class DecayingWalrus : BaseCreature
    {
        // Timer variables for unique aura ability
        private DateTime _NextDecayAura;

        [Constructable]
        public DecayingWalrus()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            this.Name = "a decaying walrus";
            this.Body = 0xDD;            // Same body as the regular walrus
            this.BaseSoundID = 0xE0;       // Same sound as the regular walrus
            this.Hue = 0x497;            // A unique, eerie hue to signal corruption

            // Significantly boosted stats compared to a simple walrus
            this.SetStr(200, 250);
            this.SetDex(150, 180);
            this.SetInt(100, 120);

            this.SetHits(500, 600);
            this.SetMana(100, 150);

            this.SetDamage(15, 30);
            // Uses a mix of physical and poison damage
            this.SetDamageType(ResistanceType.Physical, 80);
            this.SetDamageType(ResistanceType.Poison, 20);

            // Resistances: a mix of robust physical defenses and a high resistance to poison,
            // though not without its vulnerabilities.
            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 60, 70);
            this.SetResistance(ResistanceType.Energy, 30, 40);

            this.SetSkill(SkillName.MagicResist, 80.0, 100.0);
            this.SetSkill(SkillName.Tactics, 80.0, 100.0);
            this.SetSkill(SkillName.Wrestling, 80.0, 100.0);

            this.Fame = 10000;
            this.Karma = -10000;
            this.VirtualArmor = 40;



            // Initialize the decay aura timer to fire 10 seconds from spawn.
            _NextDecayAura = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        public DecayingWalrus(Serial serial)
            : base(serial)
        {
        }

        // Every now and then the Decaying Walrus releases a corrupting aura that infects nearby foes.
        public override void OnActionCombat()
        {
            // Ensure that Combatant is a Mobile before proceeding
            if (!(Combatant is Mobile combatant) || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 8) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            if (DateTime.UtcNow >= _NextDecayAura)
            {
                ReleaseDecayAura(combatant);
                // Reset timer between aura releases (15 seconds delay)
                _NextDecayAura = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            }
        }

        // The aura releases decaying energy affecting all nearby enemies and applies a potent poison.
        private void ReleaseDecayAura(Mobile target)
        {
            DoHarmful(target);

            // Affect nearby mobiles within a 2-tile radius
            foreach (Mobile m in target.GetMobilesInRange(2))
            {
                if (m == this || !CanBeHarmful(m))
                    continue;

                // Always check that we have a Mobile before accessing its properties.
                if (m is Mobile mobileTarget)
                {
                    // Apply a strong poison to simulate decay (using the built-in Deadly poison)
                    mobileTarget.ApplyPoison(this, Poison.Deadly);
                    // Deal some immediate decay damage (pure poison damage here)
                    AOS.Damage(mobileTarget, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 100, 0);
                    mobileTarget.SendMessage("You are overwhelmed by the corrupting stench of decay!");
                }
            }

            // Display a visual effect (particles) at this creature's location.
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, TimeSpan.FromSeconds(0.5)),
                0x3709, // effect item ID (example particle effect)
                10,     // speed/duration parameter
                30,     // additional effect parameter
                0x83F,  // unique hue for the effect
                0, 
                5011, 
                0);
            this.PlaySound(0xE0); // Reuse the base sound for emphasis
        }

        // When delivering melee attacks the decaying walrus may unleash its corrupting tusks.
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // 25% chance on melee hit to trigger the tusk effect.
            if (Utility.RandomDouble() < 0.25)
            {
                if (defender != null)
                {
                    // Safely check and cast to Mobile.
                    if (defender is Mobile target)
                    {
                        // Apply a potent, lethal poison to simulate the decaying effect of its tusks.
                        target.ApplyPoison(this, Poison.Lethal);
                        target.SendMessage("The decaying walrus's tusk wounds infect you with a deadly decay!");
                        // Deal extra burst damage
                        AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 100, 0, 0, 0, 0);
                    }
                }
            }
        }

        public override bool CanRummageCorpses { get { return true; } }

        // Generate loot including a chance for an additional unique drop.
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
        }

        public override int Meat { get { return 2; } }
        public override int Hides { get { return 20; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // In case you want to update the aura timer on load
            _NextDecayAura = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }
    }
}
