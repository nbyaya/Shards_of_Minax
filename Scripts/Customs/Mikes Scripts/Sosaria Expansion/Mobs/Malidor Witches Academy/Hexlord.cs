using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a cursed juggernaut corpse")]
    public class Hexlord : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextChainHexTime;
        private DateTime m_NextGlyphTime;
        private DateTime m_NextWailTime;
        private Point3D m_LastLocation;

        // Unique dark, cursed hue (adjust the number as desired)
        private const int UniqueHue = 1177;

        [Constructable]
        public Hexlord()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2) // AI and fight parameters
        {
            Name = "Hexlord";
            Body = 768;              // Uses the Juggernaut’s body
            BaseSoundID = 0x23B;      // Attack sound from the Juggernaut
            Hue = UniqueHue;

            // --- Advanced Stats ---
            SetStr(350, 450);
            SetDex(70, 90);
            SetInt(500, 600);

            SetHits(1200, 1500);
            SetMana(600, 800);
            SetStam(300, 350);

            SetDamage(15, 25);
            // Damage types: 20% physical, 50% energy, 30% poison
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 50);
            SetDamageType(ResistanceType.Poison, 30);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 80, 90);

            // --- Skills ---
            SetSkill(SkillName.EvalInt, 120.1, 130.0);
            SetSkill(SkillName.Magery, 120.1, 130.0);
            SetSkill(SkillName.MagicResist, 130.0, 140.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // --- Ability Cooldowns ---
            m_NextChainHexTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextGlyphTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextWailTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
            m_LastLocation = this.Location;

            // --- Loot: Cursed reagents ---
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool BardImmune { get { return true; } }
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int Meat { get { return 1; } }
        public override int TreasureMapLevel { get { return 6; } }

        // --- Hexing Aura on Movement ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                // Always check that the target is a Mobile before accessing its properties…
                if (m is Mobile targetMobile && SpellHelper.ValidIndirectTarget(this, targetMobile))
                {
                    DoHarmful(targetMobile);
                    targetMobile.SendMessage(0x22, "A dark curse from the Hexlord burns into your soul!");
                    targetMobile.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    PlaySound(0x1F8);
                    AOS.Damage(targetMobile, this, Utility.RandomMinMax(10, 15), 0, 0, 0, 0, 100);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            // --- Leave Behind Cursed Sigils ---
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;
                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    // Use a cursed glyph tile (implemented here as NecromanticFlamestrikeTile)
                    NecromanticFlamestrikeTile glyph = new NecromanticFlamestrikeTile();
                    glyph.Hue = UniqueHue;
                    glyph.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        NecromanticFlamestrikeTile glyph = new NecromanticFlamestrikeTile();
                        glyph.Hue = UniqueHue;
                        glyph.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // --- Ability Priority Checks ---
            if (DateTime.UtcNow >= m_NextChainHexTime && this.InRange(Combatant.Location, 10))
            {
                ChainHexCurse();
                m_NextChainHexTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextGlyphTime && this.InRange(Combatant.Location, 12))
            {
                CursedGlyphAttack();
                m_NextGlyphTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
            }
            else if (DateTime.UtcNow >= m_NextWailTime && this.InRange(Combatant.Location, 8))
            {
                HauntingWail();
                m_NextWailTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
        }

        // --- Unique Ability: Chain Hex Curse ---
        // This ability bounces between multiple targets, dealing energy damage and draining mana.
        public void ChainHexCurse()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Behold the curse!*");
            PlaySound(0x20A); // Energy bolt sound

            List<Mobile> targets = new List<Mobile>();
            Mobile initial = Combatant as Mobile;
            if (initial == null || !CanBeHarmful(initial, false) || !SpellHelper.ValidIndirectTarget(this, initial))
                return;
            targets.Add(initial);

            int maxTargets = 5;
            int range = 5;

            for (int i = 0; i < maxTargets; i++)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;

                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, range);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) &&
                        CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(lastTarget, m) &&
                        lastTarget.InLOS(m))
                    {
                        double dist = lastTarget.GetDistanceToSqrt(m);
                        if (closestDist == -1.0 || dist < closestDist)
                        {
                            closestDist = dist;
                            nextTarget = m;
                        }
                    }
                }
                eable.Free();

                if (nextTarget != null)
                    targets.Add(nextTarget);
                else
                    break;
            }

            // Apply damage, visual effects, and mana drain for each target in the chain.
            for (int i = 0; i < targets.Count; i++)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(target, false))
                    {
                        DoHarmful(target);
                        int damage = Utility.RandomMinMax(25, 40);
                        AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                        
                        // Drain mana if possible
                        if (target is Mobile targetMobile && targetMobile.Mana >= Utility.RandomMinMax(20, 30))
                        {
                            int manaDrain = Utility.RandomMinMax(20, 30);
                            targetMobile.Mana -= manaDrain;
                            targetMobile.SendMessage(0x22, "The cursed energy saps your magical reserves!");
                            targetMobile.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                            targetMobile.PlaySound(0x1F8);
                        }
                    }
                });
            }
        }

        // --- Unique Ability: Cursed Glyph Attack ---
        // This ability creates a delayed, exploding glyph at the target's location.
        public void CursedGlyphAttack()
        {
            if (Combatant == null || Map == null)
                return;

            IDamageable targetDamageable = Combatant;
            Point3D targetLocation;

            if (targetDamageable is Mobile targetMobile && CanBeHarmful(targetMobile, false))
                targetLocation = targetMobile.Location;
            else
                targetLocation = targetDamageable.Location;

            this.Say("*The curse is sealed!*");
            PlaySound(0x22F);
            Effects.SendLocationParticles(
                EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (this.Map == null)
                    return;

                Point3D spawnLoc = targetLocation;
                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                {
                    spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                        return;
                }

                NecromanticFlamestrikeTile glyph = new NecromanticFlamestrikeTile();
                glyph.Hue = UniqueHue;
                glyph.MoveToWorld(spawnLoc, this.Map);
                Effects.PlaySound(spawnLoc, this.Map, 0x1F6);
            });
        }

        // --- Unique Ability: Haunting Wail ---
        // An area effect that inflicts damage (and a chance of disorienting the target) on nearby foes.
        public void HauntingWail()
        {
            if (Map == null)
                return;

            this.Say("*Your soul shall know despair!*");
            PlaySound(0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 30, UniqueHue, 0, 5032, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(20, 35);
                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);

                // Simulate a disorienting effect with a message (stuns can be added if supported)
                if (Utility.RandomDouble() < 0.30)
                    target.SendMessage(0x22, "A wave of despair leaves you reeling!");
            }
        }

        // --- Death Effect: Final Curse Explosion ---
        // On death, the Hexlord unleashes a final burst of cursed energy, scattering hazardous ground tiles.
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The curse... unleashed!*");
                PlaySound(0x423); // Death sound (Juggernaut’s death sound)
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                int hazardsToDrop = Utility.RandomMinMax(4, 7);
                for (int i = 0; i < hazardsToDrop; i++)
                {
                    int xOffset = Utility.RandomMinMax(-4, 4);
                    int yOffset = Utility.RandomMinMax(-4, 4);
                    Point3D hazardLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                    if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                    {
                        hazardLocation.Z = Map.GetAverageZ(hazardLocation.X, hazardLocation.Y);
                        if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                            continue;
                    }

                    // Randomly choose between two hazardous tile types
                    Item tile = null;
                    if (Utility.RandomDouble() < 0.5)
                        tile = new ToxicGasTile();
                    else
                        tile = new ManaDrainTile();

                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(hazardLocation, this.Map);
                    Effects.SendLocationParticles(
                        EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration),
                        0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }
            base.OnDeath(c);
        }

        public Hexlord(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextChainHexTime);
            writer.Write(m_NextGlyphTime);
            writer.Write(m_NextWailTime);
            writer.Write(m_LastLocation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextChainHexTime = reader.ReadDateTime();
            m_NextGlyphTime = reader.ReadDateTime();
            m_NextWailTime = reader.ReadDateTime();
            m_LastLocation = reader.ReadPoint3D();
        }
    }
}
