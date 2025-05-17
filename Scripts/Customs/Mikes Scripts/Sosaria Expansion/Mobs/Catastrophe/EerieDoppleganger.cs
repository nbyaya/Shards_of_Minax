using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("an eerie doppleganger corpse")]
    public class EerieDoppleganger : BaseSABoss
    {
        // Delay timer for cycling through special abilities
        private DateTime m_NextAbilityTime;

        [Constructable]
        public EerieDoppleganger() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an eerie doppleganger";
            Title = "the Chameleon of Souls";
            Body = 0x309;
            BaseSoundID = 0x451;
            Hue = 1234; // Unique hue for the advanced monster

            // Set enhanced stats
            SetStr(350, 400);
            SetDex(150, 175);
            SetInt(200, 250);

            SetHits(800, 900);
            SetDamage(20, 30);

            // Resistances: bolstered physical and special resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Skills
            SetSkill(SkillName.MagicResist, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 95.0, 105.0);

            Fame = 8000;
            Karma = -8000;
            VirtualArmor = 75;

            Tamable = false;

            // Initialize our ability timer to trigger after 10 seconds
            m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        public EerieDoppleganger(Serial serial) : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            // Provides ultra-rich loot and some gems
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, 4);
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 8; } }

        // When damaged by a spell, occasionally trigger the shadow mirror ability
        public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster == this)
                return;
            if (caster is Mobile target && Utility.RandomDouble() < 0.20)
            {
                DoShadowMirror(target);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Ensure Combatant is a Mobile before proceeding with abilities
            if (Combatant == null || !(Combatant is Mobile target))
                return;

            if (DateTime.UtcNow >= m_NextAbilityTime)
            {
                // Randomly select one of the three abilities
                int choice = Utility.Random(3);
                switch (choice)
                {
                    case 0:
                        DoPhantasmalDrain(target);
                        break;
                    case 1:
                        DoEtherealBurst();
                        break;
                    case 2:
                        DoShadowMirror(target);
                        break;
                }

                m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }
        }

        // Phantasmal Drain:
        // This ability makes the doppleganger siphon mana and stamina from its target,
        // healing itself slightly.
        public void DoPhantasmalDrain(Mobile target)
        {
            if (target == null || target.Deleted || target.Map != this.Map)
                return;

            if (!this.CanBeHarmful(target))
                return;

            DoHarmful(target);
            // Emit a particle effect to show a ghostly siphon
            this.MovingParticles(target, 0x375A, 10, 5, false, false, 0x12D, 0, 9530, 0, 0, EffectLayer.Waist, 0);

            int drain = Utility.RandomMinMax(20, 40);
            if (target.Mana >= drain)
                target.Mana -= drain;
            else
                target.Mana = 0;

            if (target.Stam >= drain)
                target.Stam -= drain;
            else
                target.Stam = 0;

            // Heal the doppleganger for roughly half the drained amount
            this.Hits += drain / 2;
            target.SendMessage("You feel your life force being siphoned away!");
        }

        // Ethereal Burst:
        // This area–of–effect attack damages all valid targets within a 10–tile radius using a spectral shockwave.
        public void DoEtherealBurst()
        {
            Map map = this.Map;
            if (map == null)
                return;

            IPooledEnumerable eable = map.GetMobilesInRange(this.Location, 10);
            foreach (Mobile m in eable)
            {
                if (m == null || m == this)
                    continue;

                if (this.CanBeHarmful(m))
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(30, 50);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    // Emit an effect (using 0x3709 as the effect ID and our unique hue)
                    Effects.SendLocationEffect(m.Location, map, 0x3709, 30, 1234);
                }
            }
            eable.Free();
        }

        // Shadow Mirror:
        // This ability spawns an ephemeral clone near the doppleganger, confusing its enemy.
        public void DoShadowMirror(Mobile target)
        {
            if (target == null || target.Deleted || target.Map != this.Map)
                return;
            if (!this.CanBeHarmful(target))
                return;

            DoHarmful(target);
            // Show an effect at this location to indicate the mirror activation
            Effects.SendLocationEffect(this.Location, this.Map, 0x376A, 30, 1234);

            // Spawn the illusionary clone—a weaker, temporary duplicate
            EerieDopplegangerClone clone = new EerieDopplegangerClone();
            Point3D loc = new Point3D(this.X + Utility.RandomMinMax(-1, 1), this.Y + Utility.RandomMinMax(-1, 1), this.Z);
            clone.MoveToWorld(loc, this.Map);
            clone.Combatant = target;
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);
        }

        // Reflects damage from melee attacks coming from BaseCreatures back to them.
        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from is BaseCreature)
            {
                if (Utility.RandomDouble() < 0.7)
                    from.Damage(damage, this);
            }
        }

        // (Optional) Lists of unique and shared special abilities.
        // These can be used by systems that track the abilities of advanced monsters.
        public override Type[] UniqueSAList
        {
            get
            {
                return new Type[]
                {
                    typeof(PhantasmalDrain),
                    typeof(EtherealBurst),
                    typeof(ShadowMirror)
                };
            }
        }

        public override Type[] SharedSAList
        {
            get
            {
                return new Type[]
                {
                    // Add any shared abilities here if needed
                };
            }
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool Unprovokable { get { return false; } }
        public override bool BardImmune { get { return false; } }
        public override bool CanFlee { get { return false; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // ------------------------------------------------------------------------
    // Illusory clone class used by the Shadow Mirror ability.
    // This creature is a weak duplicate of the Eerie Doppleganger and will vanish shortly.
    public class EerieDopplegangerClone : BaseCreature
    {
        [Constructable]
        public EerieDopplegangerClone() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadowy illusion";
            Body = 0x309;
            BaseSoundID = 0x451;
            Hue = 1234; // Same unique hue

            // Much weaker stats for the clone
            SetStr(50);
            SetDex(50);
            SetInt(50);

            SetHits(100);
            SetDamage(5, 10);

            VirtualArmor = 25;
            // The clone is not meant to persist; it is a temporary decoy.
        }

        public EerieDopplegangerClone(Serial serial) : base(serial)
        {
        }

        // Indicates that the clone should be automatically deleted when released.
        public override bool DeleteOnRelease { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();
            // Optionally, add logic so that the clone deletes itself after a set duration.
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

    // Dummy classes for ability type references.
    // These are placeholders so that the UniqueSAList is non-empty.
    public class PhantasmalDrain { }
    public class EtherealBurst { }
    public class ShadowMirror { }
}
