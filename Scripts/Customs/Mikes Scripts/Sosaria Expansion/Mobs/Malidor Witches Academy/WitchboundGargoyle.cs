using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;          // For spell effects, if needed
using Server.Network;         // For playing effects and sounds
using System.Collections.Generic; // For lists in AoE targeting

namespace Server.Mobiles
{
    [CorpseName("a witchbound gargoyle corpse")]
    public class WitchboundGargoyle : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextWailTime;
        private DateTime m_NextWingBlastTime;

        // Unique hue for our magic-themed gargoyle (distinct from the base 1153)
        private const int UniqueHue = 1175;

        // Track last location if you wish to later add movement-based effects
        private Point3D m_LastLocation;

        [Constructable]
        public WitchboundGargoyle() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Witchbound Gargoyle";
            Body = 0x2F3;       // Based on GargoyleGuardian
            BaseSoundID = 0x174; // Same as GargoyleGuardian
            Hue = UniqueHue;

            // --- Advanced Stats and Pools ---
            SetStr(800, 900);
            SetDex(120, 150);
            SetInt(400, 450);

            SetHits(1200, 1400);
            SetStam(300, 350);
            SetMana(800, 900);

            // Baseline damage is moderate; unique abilities add extra harm.
            SetDamage(50, 60);
            // Damage types: 20% physical, 80% energy
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 80, 90);

            // --- Skills: Emphasis on magic ---
            SetSkill(SkillName.Magery, 110.1, 125.0);
            SetSkill(SkillName.EvalInt, 110.1, 125.0);
            SetSkill(SkillName.MagicResist, 120.2, 135.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 80;
            ControlSlots = 5;

            // --- Ability Cooldowns ---
            m_NextWailTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextWingBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            m_LastLocation = this.Location;
        }

        // --- Aura Effect: Arcane Drain ---
        // Whenever any creature comes close (within 2 tiles), drain mana and deal slight energy damage.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && InRange(m.Location, 2) && CanBeHarmful(m, false))
            {
                // Before accessing Mobile-specific properties, check type
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int manaDrain = Utility.RandomMinMax(10, 20);
                    if (target.Mana >= manaDrain)
                    {
                        target.Mana -= manaDrain;
                        target.SendMessage(0x22, "A mysterious force drains your magic!");
                        target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                        target.PlaySound(0x1F8);
                    }
                    // Inflict a little extra energy damage
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- OnThink Override: Periodically use special attacks ---
        public override void OnThink()
        {
            base.OnThink();

            // Make sure we have a valid combatant. Check if Combatant is a Mobile before using its properties.
            Mobile combatant = Combatant as Mobile;
            if (combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // If within 8 tiles, try the Witchbound Wail Attack
            if (DateTime.UtcNow >= m_NextWailTime && InRange(combatant.Location, 8))
            {
                WitchboundWailAttack();
                m_NextWailTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // Otherwise, if within 5 tiles, try the Wing Blast Attack
            else if (DateTime.UtcNow >= m_NextWingBlastTime && InRange(combatant.Location, 5))
            {
                WingBlastAttack();
                m_NextWingBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
        }

        // --- Ability 1: Witchbound Wail Attack ---
        // Unleashes a terrifying wail that deals energy damage in an AoE and drains mana.
        public void WitchboundWailAttack()
        {
            PlaySound(0x20F); // A wailing sound effect
            FixedParticles(0x3709, 15, 25, 5052, UniqueHue, 0, EffectLayer.Waist);
            this.Say("*The gargoyle unleashes a soul-wrenching wail!*");

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(35, 55);
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100); // Deal energy damage
                    target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    
                    // Drain additional mana from the target (if it is a Mobile)
                    if (target is Mobile mTarget)
                    {
                        int extraDrain = Utility.RandomMinMax(20, 30);
                        if (mTarget.Mana >= extraDrain)
                        {
                            mTarget.Mana -= extraDrain;
                            mTarget.SendMessage(0x22, "The wail siphons your magical essence!");
                            mTarget.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                            mTarget.PlaySound(0x1F8);
                        }
                    }
                }
            }
        }

        // --- Ability 2: Wing Blast Attack ---
        // A close-range physical burst that deals damage and attempts to knock targets back.
        public void WingBlastAttack()
        {
            PlaySound(0x1FD); // A gust or impact sound
            FixedParticles(0x376A, 15, 20, 5045, UniqueHue, 0, EffectLayer.Waist);
            this.Say("*The gargoyle slams its wings in a furious gust!*");

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 5);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(25, 40);
                // Deal 100% physical damage
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                target.FixedParticles(0x376A, 10, 15, 5045, UniqueHue, 0, EffectLayer.Waist);
                target.SendMessage(0x22, "The force of the wing blast knocks you off balance!");

                // Attempt a rudimentary knockback: move the target one tile away if possible
                Point3D direction = new Point3D(target.Location.X - this.Location.X, target.Location.Y - this.Location.Y, 0);
                int offsetX = (direction.X != 0) ? direction.X / Math.Abs(direction.X) : 0;
                int offsetY = (direction.Y != 0) ? direction.Y / Math.Abs(direction.Y) : 0;
                Point3D newLoc = new Point3D(target.Location.X + offsetX, target.Location.Y + offsetY, target.Location.Z);
                if (Map.CanFit(newLoc.X, newLoc.Y, newLoc.Z, 16, false, false))
                    target.Location = newLoc;
            }
        }

        // --- Ability 3: Spell Reflection ---
        // When hit by a spell, there is a chance to reflect some of the damage back at the caster.
        public override void OnDamagedBySpell(Mobile from)
        {
            base.OnDamagedBySpell(from);
            if (from != null && from.Alive && Utility.RandomDouble() < 0.3)
            {
                if (from is Mobile target)
                {
                    this.SendMessage("The Witchbound Gargoyle reflects your spell!");
                    // Reflect a set amount of damage (adjust value as needed)
                    AOS.Damage(target, this, 25, 0, 0, 0, 0, 100);
                    target.FixedParticles(0x376A, 10, 15, 5045, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x1F8);
                }
            }
        }

        // --- Death Effect: Witchfire Explosion ---
        // On death, the gargoyle erupts in a burst of magical witchfire,
        // spawning several hazardous ground tiles chosen from a small selection.
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The gargoyle's cursed magic erupts in a burst of witchfire!*");
                PlaySound(0x20C); // Explosion sound effect
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 15, 25, UniqueHue, 0, 5052, 0);

                // Drop between 4 and 6 hazardous tiles around its corpse.
                int hazardsToDrop = Utility.RandomMinMax(4, 6);
                for (int i = 0; i < hazardsToDrop; i++)
                {
                    int offsetX = Utility.RandomMinMax(-3, 3);
                    int offsetY = Utility.RandomMinMax(-3, 3);
                    Point3D hazardLoc = new Point3D(X + offsetX, Y + offsetY, Z);
                    
                    if (!Map.CanFit(hazardLoc.X, hazardLoc.Y, hazardLoc.Z, 16, false, false))
                    {
                        hazardLoc.Z = Map.GetAverageZ(hazardLoc.X, hazardLoc.Y);
                        if (!Map.CanFit(hazardLoc.X, hazardLoc.Y, hazardLoc.Z, 16, false, false))
                            continue;
                    }
                    
                    // Randomly choose one hazardous tile type for flavor:
                    int choice = Utility.RandomMinMax(0, 2);
                    Item hazardTile = null;
                    switch (choice)
                    {
                        case 0:
                            hazardTile = new NecromanticFlamestrikeTile();
                            break;
                        case 1:
                            hazardTile = new PoisonTile();
                            break;
                        case 2:
                            hazardTile = new ThunderstormTile();
                            break;
                    }
                    if (hazardTile != null)
                    {
                        hazardTile.Hue = UniqueHue;
                        hazardTile.MoveToWorld(hazardLoc, Map);
                    }
                }
            }
            base.OnDeath(c);
        }

        // --- Standard Overrides and Loot ---
        public override bool BardImmune { get { return !Core.AOS; } }
        public override int Meat { get { return 1; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(2, 4));
            // A chance for a unique magical drop
            if (Utility.RandomDouble() < 0.02)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public WitchboundGargoyle(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextWailTime);
            writer.Write(m_NextWingBlastTime);
            writer.Write(m_LastLocation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextWailTime = reader.ReadDateTime();
            m_NextWingBlastTime = reader.ReadDateTime();
            m_LastLocation = reader.ReadPoint3D();
        }
    }
}
